using AlbionMarshaller;
using ExitGames.Client.Photon;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using static AlbionMarshaller.BaseEvent;

namespace AlbionProcessor
{
    public class PacketHandler
    {
        public readonly string LogTimer = DateTime.UtcNow.ToString("dd-MMM-HH-mm-ss");
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<String, Boolean> _loggerAttached = new Dictionary<string, bool>();

        public ConcurrentDictionary<String, int> triggeredEvents = new ConcurrentDictionary<string, int>();
        public ConcurrentDictionary<String, int> triggeredOperations = new ConcurrentDictionary<string, int>();

        private Dictionary<EventCodes, List<HandleEvent>> _eventHandlers = new Dictionary<EventCodes, List<HandleEvent>>();
        private Dictionary<OperationCodes, List<HandleOperation>> _requestHandlers = new Dictionary<OperationCodes, List<HandleOperation>>();
        private Dictionary<OperationCodes, List<HandleOperation>> _responseHandlers = new Dictionary<OperationCodes, List<HandleOperation>>();

        private bool _initialized = false;

        public PacketHandler()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (!_initialized)
            {
                lock (this)
                {
                    new Thread(delegate ()
                    {
                        this.CreateListener();
                    }).Start();

                    var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
                    var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

                    var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
                    var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

                    toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

                    var methods = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()
                                                                          .SelectMany(t => t.GetMethods())
                                                                          .Where(m => m.GetCustomAttributes(typeof(AlbionMarshaller.EventHandler), false).Length > 0)
                                                                          .ToArray()).ToArray();
                    foreach (MethodInfo method in methods)
                    {
                        var del = (HandleEvent)Delegate.CreateDelegate(typeof(HandleEvent), method);
                        foreach (CustomAttributeData attributeData in method.CustomAttributes)
                        {
                            EventCodes eventCode = (EventCodes)attributeData.ConstructorArguments[0].Value;
                            if (!_eventHandlers.ContainsKey(eventCode))
                            {
                                _eventHandlers.Add(eventCode, new List<HandleEvent>());
                            }
                            _eventHandlers[eventCode].Add(del);
                        }
                    }

                    methods = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()
                                                                      .SelectMany(t => t.GetMethods())
                                                                      .Where(m => m.GetCustomAttributes(typeof(OperationHandler), false).Length > 0)
                                                                      .ToArray()).ToArray();
                    foreach (MethodInfo method in methods)
                    {
                        var del = (HandleOperation)Delegate.CreateDelegate(typeof(HandleOperation), method);
                        foreach (CustomAttributeData attributeData in method.CustomAttributes)
                        {
                            OperationCodes opCode = (OperationCodes)attributeData.ConstructorArguments[0].Value;
                            OperationType opType = (OperationType)attributeData.ConstructorArguments[1].Value;
                            if (opType == OperationType.Request)
                            {
                                if (!_requestHandlers.ContainsKey(opCode))
                                {
                                    _requestHandlers.Add(opCode, new List<HandleOperation>());
                                }
                                _requestHandlers[opCode].Add(del);
                            }
                            else
                            {
                                if (!_responseHandlers.ContainsKey(opCode))
                                {
                                    _responseHandlers.Add(opCode, new List<HandleOperation>());
                                }
                                _responseHandlers[opCode].Add(del);
                            }
                        }
                    }

                    // Technically not initialized but everything after this is internal only
                    _initialized = true;
                }
            }
        }

        private static readonly Lazy<PacketHandler> lazy = new Lazy<PacketHandler>(() => new PacketHandler());
        public static PacketHandler Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private static RollingFileAppender CreateRollingFileAppender(string name)
        {
            var layout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger [%property{NDC}] - %message%newline"
            };
            layout.ActivateOptions();

            return new RollingFileAppender
            {
                Name = name,
                AppendToFile = true,
                DatePattern = "yyyyMMdd",
                MaximumFileSize = "1MB",
                MaxSizeRollBackups = 10,
                RollingStyle = RollingFileAppender.RollingMode.Composite,
                File = $"logs\\{name}_log.txt",
                Layout = layout,
                Threshold=Level.Debug
            };
        }

        public void OnEvent(byte code, Dictionary<byte, object> parameters)
        {
            if (code == 2)
            {
                return;
            }
            object val;
            parameters.TryGetValue(252, out val);
            if (val == null)
            {
                return;
            }
            int iCode = 0;
            if (!int.TryParse(val.ToString(), out iCode))
            {
                return;
            }

            EventCodes eventCode = (EventCodes)iCode;
            string eventCodeStr = eventCode.ToString();
            if (triggeredEvents.ContainsKey(eventCodeStr))
            {
                triggeredEvents[eventCodeStr] += 1;
            }
            else
            {
                triggeredEvents[eventCodeStr] = 1;
            }

            /*if (eventCode.ToString().StartsWith("Cast") || eventCode.ToString().StartsWith("Attack") || eventCode.ToString().StartsWith("Health") 
                || eventCode.ToString().Contains("Spell") || eventCode.ToString().Contains("Mount") || eventCode.ToString().Contains("Energy") 
                || eventCode.ToString().Contains("Guild") || eventCode.ToString().Contains("Harvest") || eventCode.ToString().Contains("Channeling"))
            {
                return;
            }*/

            string loggerName = "Event." + eventCode.ToString();
            ILog log = LogManager.GetLogger(loggerName);
            log.Debug(parameters);
            if (_eventHandlers.ContainsKey(eventCode))
            {
                foreach (HandleEvent eventHandler in _eventHandlers[eventCode])
                {
                    try
                    {
                        eventHandler(parameters, log);
                    }
                    catch(Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
        }

        public void OnResponse(byte operationCode, short returnCode, Dictionary<byte, object> parameters)
        {
            int iCode = 0;
            LogManager.GetLogger("RAW").Debug(parameters);
            if (int.TryParse(parameters[253].ToString(), out iCode))
            {
                OperationCodes opCode = (OperationCodes)iCode;
                if (opCode.ToString().StartsWith("Move"))
                {
                    return;
                }
                string loggerName = "Response." + opCode.ToString();
                ILog log = LogManager.GetLogger(loggerName);
                log.Debug(parameters);
                if (_responseHandlers.ContainsKey(opCode))
                {
                    foreach (HandleOperation opHandler in _responseHandlers[opCode])
                    {
                        try
                        {
                            opHandler(parameters, log);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                }
            }
        }

        public void OnRequest(byte operationCode, Dictionary<byte, object> parameters)
        {
            int iCode = 0;
            if (int.TryParse(parameters[253].ToString(), out iCode))
            {
                OperationCodes opCode = (OperationCodes)iCode;
                if (opCode.ToString().StartsWith("Move"))
                {
                    return;
                }
                string loggerName = "Request." + opCode.ToString();
                ILog log = LogManager.GetLogger(loggerName);
                log.Debug(parameters);

                if (_requestHandlers.ContainsKey(opCode))
                {
                    foreach (HandleOperation opHandler in _requestHandlers[opCode])
                    {
                        try
                        {
                            opHandler(parameters, log);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                }
            }
        }

        public void Shutdown()
        {
            var allDevices = CaptureDeviceList.Instance;
            for (int i = 0; i != allDevices.Count; i++)
            {
                ICaptureDevice device = allDevices[i];

                device.StopCapture();
                device.Close();
            }
        }

        private void CreateListener()
        {
            var allDevices = CaptureDeviceList.Instance;
            if (allDevices.Count == 0)
            {
                log.Debug("No Network Interface Found! Please make sure WinPcap is properly installed.");
                return;
            }
            for (int i = 0; i != allDevices.Count; i++)
            {
                ICaptureDevice device = allDevices[i];

                if (device.Description != null)
                {
                    Debug.WriteLine(" (" + device.Description + ")");
                }
                else
                {
                    Debug.WriteLine(" (Unknown)");
                }

                device.OnPacketArrival += new PacketArrivalEventHandler(PacketHandle);
                device.Open(DeviceMode.Promiscuous, 1000);
                device.Filter = "ip and udp and (port 5056 or port 5055 or port 4535)";
                if (device.LinkType != LinkLayers.Ethernet)
                {
                    device.Close();
                    continue;
                }
                device.StartCapture();
            }
        }

        private void PacketHandle(object sender, CaptureEventArgs e)
        {
            Packet packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            var udpPacket = packet.Extract<UdpPacket>();
            var ipPacket = packet.Extract<IPPacket>();

            if (udpPacket == null)
            {
                return;
            }
            
            //ILog plog = LogManager.GetLogger("Packet");
            //plog.Debug(string.Format("{0}:{1}->{2}:{3}", ipPacket.SourceAddress, udpPacket.SourcePort, ipPacket.DestinationAddress, udpPacket.DestinationPort));
            String hexPayloadData = Convert.ToBase64String(udpPacket.PayloadData);
            Protocol16 protocol16 = new Protocol16();
            BinaryReader binaryReader = new BinaryReader(new MemoryStream(udpPacket.PayloadData));
            try
            {
                IPAddress.NetworkToHostOrder((int)binaryReader.ReadUInt16());
                binaryReader.ReadByte();
                byte commandCount = binaryReader.ReadByte();
                IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
                IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
                int commandHeaderLength = 12;
                int signifierByteLength = 1;
                for (int commandIdx = 0; commandIdx < (int)commandCount; commandIdx++)
                {
                    try
                    {
                        byte commandType = binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        int commandLength = IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
                        IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
                        switch (commandType)
                        {
                            case 4:
                                goto IL_1E7;
                            case 5:
                                goto IL_1CF;
                            case 6:
                                break;
                            case 7:
                                binaryReader.BaseStream.Position += 4L;
                                commandLength -= 4;
                                break;
                            default:
                                goto IL_1CF;
                        }
                        binaryReader.BaseStream.Position += (long)signifierByteLength;
                        byte messageType = binaryReader.ReadByte();
                        int operationLength = commandLength - commandHeaderLength - 2;
                        StreamBuffer payload = new StreamBuffer(binaryReader.ReadBytes(operationLength));
                        switch (messageType)
                        {
                            case 2:
                                {
                                    //ILog rlog = LogManager.GetLogger("Request");
                                    //rlog.Debug(hexPayloadData);
                                    OperationRequest requestData = protocol16.DeserializeOperationRequest(payload);
                                    Instance.OnRequest(requestData.OperationCode, requestData.Parameters);
                                    break;
                                }
                            case 3:
                                {
                                    //ILog relog = LogManager.GetLogger("Response");
                                    //relog.Debug(hexPayloadData);
                                    OperationResponse responseData = protocol16.DeserializeOperationResponse(payload);
                                    Instance.OnResponse(responseData.OperationCode, responseData.ReturnCode, responseData.Parameters);
                                    break;
                                }
                            case 4:
                                {
                                    //ILog elog = LogManager.GetLogger("Event");
                                    //elog.Debug(hexPayloadData);
                                    EventData eventData = protocol16.DeserializeEventData(payload);
                                    Instance.OnEvent(eventData.Code, eventData.Parameters);
                                    break;
                                }
                            default:
                                //ILog olog = LogManager.GetLogger("Other");
                                //olog.Debug(hexPayloadData);
                                binaryReader.BaseStream.Position += (long)operationLength;
                                break;
                        }
                        payload.Close();
                        goto IL_1E7;
                        IL_1CF:
                        binaryReader.BaseStream.Position += (long)(commandLength - commandHeaderLength);
                        IL_1E7:;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }
    }
}
