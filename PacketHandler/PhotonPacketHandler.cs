using log4net;
using PacketDotNet;
using PhotonPackageParser;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static AlbionMarshaller.BaseEvent;

namespace AlbionMarshaller
{
    public class PhotonPacketHandler : PhotonParser
    {
        public delegate byte[] HandleSpecial(byte[] payload, ILog log);

        public readonly string LogTimer = DateTime.UtcNow.ToString("dd-MMM-HH-mm-ss");
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<EventCodes, List<HandleEvent>> _eventHandlers = new Dictionary<EventCodes, List<HandleEvent>>();
        private Dictionary<OperationCodes, List<HandleOperation>> _requestHandlers = new Dictionary<OperationCodes, List<HandleOperation>>();
        private List<HandleOperation> _specialRequestHandlers = new List<HandleOperation>();
        private Dictionary<OperationCodes, List<HandleOperation>> _responseHandlers = new Dictionary<OperationCodes, List<HandleOperation>>();
        private List<HandleOperation> _specialResponseHandlers = new List<HandleOperation>();

        private HandleSpecial _specialHandler = null;

        private bool _initialized = false;
        public PhotonPacketHandler()
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
                            bool special = (bool)attributeData.ConstructorArguments[2].Value;
                            if (opType == OperationType.Request)
                            {
                                if (special)
                                {
                                    _specialRequestHandlers.Add(del);
                                }
                                else
                                {
                                    if (!_requestHandlers.ContainsKey(opCode))
                                    {
                                        _requestHandlers.Add(opCode, new List<HandleOperation>());
                                    }
                                    _requestHandlers[opCode].Add(del);
                                }
                            }
                            else
                            {
                                if (special)
                                {
                                    _specialResponseHandlers.Add(del);
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
                    }

                    methods = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()
                                                                      .SelectMany(t => t.GetMethods())
                                                                      .Where(m => m.GetCustomAttributes(typeof(SpecialHandler), false).Length > 0)
                                                                      .ToArray()).ToArray();
                    foreach (MethodInfo method in methods)
                    {
                        var del = (HandleSpecial)Delegate.CreateDelegate(typeof(HandleSpecial), method);
                        _specialHandler = del;
                    }

                    // Technically not initialized but everything after this is internal only
                    _initialized = true;
                }
            }
        }

        private static readonly Lazy<PhotonPacketHandler> lazy = new Lazy<PhotonPacketHandler>(() => new PhotonPacketHandler());
        public static PhotonPacketHandler Instance
        {
            get
            {
                return lazy.Value;
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

            try
            {
                ReceivePacket(udpPacket.PayloadData);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
        }        

        protected override void OnEvent(byte code, Dictionary<byte, object> parameters)
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

            string loggerName = "Event." + eventCode.ToString();

            ILog log = LogManager.GetLogger(loggerName);
            #if DEBUG
                log.Debug(parameters);
            #endif

            if (_eventHandlers.ContainsKey(eventCode))
            {
                foreach (HandleEvent eventHandler in _eventHandlers[eventCode])
                {
                    try
                    {
                        eventHandler(parameters, log);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
        }

        protected override void OnRequest(byte operationCode, Dictionary<byte, object> parameters)
        {
#if DEBUG
            LogManager.GetLogger("RAW").Debug(parameters);
#endif
            if (operationCode == 0)
            {
                foreach (HandleOperation opHandler in _specialRequestHandlers)
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
            else
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
#if DEBUG
                    log.Debug(parameters);
#endif

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
        }

        protected override byte[] OnSpecial(byte[] bytes)
        {
            if (_specialHandler == null) return new byte[0];
            return _specialHandler(bytes, log);
        }

        protected override void OnResponse(byte operationCode, short returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
#if DEBUG
            LogManager.GetLogger("RAW").Debug(parameters);
#endif
            if (operationCode == 0)
            {
                foreach (HandleOperation opHandler in _specialResponseHandlers)
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
            else
            {
                int iCode = 0;
                if (int.TryParse(parameters[253].ToString(), out iCode))
                {
                    OperationCodes opCode = (OperationCodes)iCode;
                    if (opCode.ToString().StartsWith("Move"))
                    {
                        return;
                    }
                    string loggerName = "Response." + opCode.ToString();
                    ILog log = LogManager.GetLogger(loggerName);
#if DEBUG
                    log.Debug(parameters);
#endif
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
        }
    }
}
