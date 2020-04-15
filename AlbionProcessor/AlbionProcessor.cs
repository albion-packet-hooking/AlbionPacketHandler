using AlbionProcessor.MemoryStorage;
using AlbionProcessor.Model;
using ExitGames.Client.Photon;
using PacketDotNet;
using SharpPcap;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace AlbionProcessor
{
    public class AlbionProcessor
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AlbionProcessor()
        {
            new Thread(delegate ()
            {
                this.CreateListener();
            }).Start();

            LootDB.Instance.LootAddedToPlayer += PlayerLootAdded;
        }

        private void PlayerLootAdded(object sender, PlayerLootEventArgs plea)
        {
            Loot item = plea.Loot;
            Player player = plea.Player;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "aolootlog", PacketHandler.Instance.LogTimer + ".csv");
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string logMessage = $"[{item.PickupTime.ToString()}] {player.Name} has looted {item.Quantity}x {item.ItemName} from {item.BodyName}";
            string csvMessage = $"{item.PickupTime.ToString()};{player.Name};{item.ItemName};{item.Quantity};{item.BodyName}";

            log.Info(logMessage);
            using (StreamWriter streamWriter = File.AppendText(path))
            {
                streamWriter.WriteLine(csvMessage);
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
                device.Filter = "ip and udp and port 5056";
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

            if (udpPacket == null)
            {
                return;
            }

            Protocol16 protocol16 = new Protocol16();
            if (udpPacket.SourcePort != 5056 && udpPacket.DestinationPort != 5056)
            {
                return;
            }
            BinaryReader binaryReader = new BinaryReader(new MemoryStream(udpPacket.PayloadData));
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
                                OperationRequest requestData = protocol16.DeserializeOperationRequest(payload);
                                PacketHandler.Instance.OnRequest(requestData.OperationCode, requestData.Parameters);
                                goto IL_1E7;
                            }
                        case 3:
                            {
                                OperationResponse responseData = protocol16.DeserializeOperationResponse(payload);
                                PacketHandler.Instance.OnResponse(responseData.OperationCode, responseData.ReturnCode, responseData.Parameters);
                                goto IL_1E7;
                            }
                        case 4:
                            {
                                EventData eventData = protocol16.DeserializeEventData(payload);
                                PacketHandler.Instance.OnEvent(eventData.Code, eventData.Parameters);
                                goto IL_1E7;
                            }
                        default:
                            binaryReader.BaseStream.Position += (long)operationLength;
                            goto IL_1E7;
                    }
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
    }
}
