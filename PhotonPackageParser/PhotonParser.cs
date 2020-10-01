using Protocol16;
using System;
using System.Collections.Generic;
using System.IO;
using Protocol16.Photon;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Threading;
using Org.BouncyCastle.Math;

namespace PhotonPackageParser
{
    public abstract class PhotonParser
    {
        public static int TickCount { get; set; } = int.MinValue;

        private const int CommandHeaderLength = 12;
        private const int PhotonHeaderLength = 12;

        private readonly Dictionary<int, SegmentedPackage> _pendingSegments = new Dictionary<int, SegmentedPackage>();

        public void ReceivePacket(byte[] payload)
        {
            if (payload.Length < PhotonHeaderLength)
            {
                return;
            }

            TickCount = Environment.TickCount;
            //TickCount = 146722234;

            int offset = 0;
            NumberDeserializer.Deserialize(out short peerId, payload, ref offset);
            ReadByte(out byte flags, payload, ref offset);
            ReadByte(out byte commandCount, payload, ref offset);
            NumberDeserializer.Deserialize(out int timestamp, payload, ref offset);
            NumberDeserializer.Deserialize(out int challenge, payload, ref offset);

            bool isEncrypted = flags == 1;
            bool isCrcEnabled = flags == 0xCC;

            if (isEncrypted)
            {
                // Encrypted packages are not supported
                return;
            }

            if (isCrcEnabled)
            {
                int ignoredOffset = 0;
                NumberDeserializer.Deserialize(out int crc, payload, ref ignoredOffset);
                NumberSerializer.Serialize(0, payload, ref offset);

                if (crc != CrcCalculator.Calculate(payload, payload.Length))
                {
                    // Invalid crc
                    return;
                }
            }

            for (int commandIdx = 0; commandIdx < commandCount; commandIdx++)
            {
                HandleCommand(payload, ref offset);
            }
        }

        protected abstract void OnRequest(byte operationCode, Dictionary<byte, object> parameters);

        protected abstract void OnResponse(byte operationCode, short returnCode, string debugMessage, Dictionary<byte, object> parameters);

        protected abstract void OnEvent(byte code, Dictionary<byte, object> parameters);
        protected abstract byte[] OnSpecial(byte[] bytes);

        private void HandleCommand(byte[] source, ref int offset)
        {
            ReadByte(out byte commandType, source, ref offset);
            ReadByte(out byte channelId, source, ref offset);
            ReadByte(out byte commandFlags, source, ref offset);
            // Skip 1 byte
            offset++;
            NumberDeserializer.Deserialize(out int commandLength, source, ref offset);
            NumberDeserializer.Deserialize(out int sequenceNumber, source, ref offset);
            commandLength -= CommandHeaderLength;

            switch ((CommandType)commandType)
            {
                case CommandType.Disconnect:
                    {
                        return;
                    }
                case CommandType.SendUnreliable:
                    {
                        offset += 4;
                        commandLength -= 4;
                        goto case CommandType.SendReliable;
                    }
                case CommandType.SendReliable:
                    {
                        HandleSendReliable(source, ref offset, ref commandLength);
                        break;
                    }
                case CommandType.SendFragment:
                    {
                        HandleSendFragment(source, ref offset, ref commandLength);
                        break;
                    }
                default:
                    {
                        offset += commandLength;
                        break;
                    }
            }
        }

        private void HandleSendReliable(byte[] source, ref int offset, ref int commandLength)
        {
            if(commandLength < 0)
            {
                Debug.WriteLine("Something wrong with packet | content=" + Convert.ToBase64String(source));                
            }

            byte[] payloadBytes = new byte[commandLength];
            Array.Copy(source, offset, payloadBytes, 0, commandLength);
            // Skip 1 byte
            offset += commandLength;

            int index = 0;
            ReadByte(out byte flag, payloadBytes, ref index);

            bool flag3 = flag != 243 && flag != 253;
            if(flag3)
            {
                return;
            }

            ReadByte(out byte messageDeets, payloadBytes, ref index);
            byte messageType = (byte)(messageDeets & 127);
            bool special = (messageDeets & 128) > 0;

            Protocol16Stream payload;
            if (!special)
            {
                int operationLength = payloadBytes.Length - index;
                payload = new Protocol16Stream(operationLength);
                payload.Write(payloadBytes, index, operationLength);
                payload.Seek(0L, SeekOrigin.Begin);
            }
            else
            {
                byte[] actualContent = new byte[payloadBytes.Length - index];
                Array.Copy(payloadBytes, index, actualContent, 0, actualContent.Length);
                byte[] result = OnSpecial(actualContent);

                if (result.Length == 0) return;

                int operationLength = result.Length;
                payload = new Protocol16Stream(operationLength);
                payload.Write(result, 0, operationLength);
                payload.Seek(0L, SeekOrigin.Begin);
            }

            switch ((MessageType)messageType)
            {
                case MessageType.OperationRequest:
                    {
                        OperationRequest requestData = Protocol16Deserializer.DeserializeOperationRequest(payload);
                        OnRequest(requestData.OperationCode, requestData.Parameters);
                        break;
                    }
                case MessageType.OperationResponse:
                    {
                        OperationResponse responseData = Protocol16Deserializer.DeserializeOperationResponse(payload);
                        OnResponse(responseData.OperationCode, responseData.ReturnCode, responseData.DebugMessage, responseData.Parameters);
                        break;
                    }
                case MessageType.Event:
                    {
                        EventData eventData = Protocol16Deserializer.DeserializeEventData(payload);
                        OnEvent(eventData.Code, eventData.Parameters);
                        break;
                    }
                case MessageType.InternalOperationRequest:
                    {
                        OperationRequest intReqData = Protocol16Deserializer.DeserializeOperationRequest(payload);
                        intReqData.Parameters.Add(88, TickCount);
                        OnRequest(intReqData.OperationCode, intReqData.Parameters);
                        break;
                    }
                case MessageType.InternalOperationResponse:
                    {
                        OperationResponse intResponseData = Protocol16Deserializer.DeserializeOperationResponse(payload);
                        OnResponse(intResponseData.OperationCode, intResponseData.ReturnCode, intResponseData.DebugMessage, intResponseData.Parameters);
                        break;
                    }
            }
        }

        private void HandleSendFragment(byte[] source, ref int offset, ref int commandLength)
        {
            NumberDeserializer.Deserialize(out int startSequenceNumber, source, ref offset);
            commandLength -= 4;
            NumberDeserializer.Deserialize(out int fragmentCount, source, ref offset);
            commandLength -= 4;
            NumberDeserializer.Deserialize(out int fragmentNumber, source, ref offset);
            commandLength -= 4;
            NumberDeserializer.Deserialize(out int totalLength, source, ref offset);
            commandLength -= 4;
            NumberDeserializer.Deserialize(out int fragmentOffset, source, ref offset);
            commandLength -= 4;

            int fragmentLength = commandLength;
            HandleSegmentedPayload(startSequenceNumber, totalLength, fragmentLength, fragmentOffset, source, ref offset);
        }

        private void HandleFinishedSegmentedPackage(byte[] totalPayload)
        {
            int offset = 0;
            int commandLength = totalPayload.Length;
            HandleSendReliable(totalPayload, ref offset, ref commandLength);
        }

        private void HandleSegmentedPayload(int startSequenceNumber, int totalLength, int fragmentLength, int fragmentOffset, byte[] source, ref int offset)
        {
            SegmentedPackage segmentedPackage = GetSegmentedPackage(startSequenceNumber, totalLength);

            Buffer.BlockCopy(source, offset, segmentedPackage.TotalPayload, fragmentOffset, fragmentLength);
            offset += fragmentLength;
            segmentedPackage.BytesWritten += fragmentLength;

            if (segmentedPackage.BytesWritten >= segmentedPackage.TotalLength)
            {
                _pendingSegments.Remove(startSequenceNumber);
                HandleFinishedSegmentedPackage(segmentedPackage.TotalPayload);
            }
        }

        private SegmentedPackage GetSegmentedPackage(int startSequenceNumber, int totalLength)
        {
            if (_pendingSegments.TryGetValue(startSequenceNumber, out SegmentedPackage segmentedPackage))
            {
                return segmentedPackage;
            }

            segmentedPackage = new SegmentedPackage
            {
                TotalLength = totalLength,
                TotalPayload = new byte[totalLength],
            };
            _pendingSegments.Add(startSequenceNumber, segmentedPackage);

            return segmentedPackage;
        }

        private static void ReadByte(out byte value, byte[] source, ref int offset)
        {
            value = source[offset++];
        }
    }
}
