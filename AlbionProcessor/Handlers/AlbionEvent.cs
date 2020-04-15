using log4net;
using AlbionProcessor.MemoryStorage;
using AlbionProcessor.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace AlbionProcessor
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class EventHandler : Attribute
    {
        public EventCodes EventCode { get; set; }
        public EventHandler(EventCodes eventCode)
        {
            EventCode = eventCode;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class OperationHandler : Attribute
    {
        public OperationCodes OpCode { get; set; }
        public OperationHandler(OperationCodes opCode)
        {
            OpCode = opCode;
        }
    }

    public abstract class AlbionEvent
    {
        public delegate void HandleEvent(Dictionary<byte, object> parameters, ILog log);
        public delegate void HandleOperation(Dictionary<byte, object> parameters, ILog log);

        protected static int[] Convert(object intArray)
        {
            if (intArray is byte[])
            {
                return Array.ConvertAll((byte[])intArray, b => (int)b);
            }
            else if (intArray is short[])
            {
                return Array.ConvertAll((short[])intArray, b => (int)b);
            }
            else if (intArray is int[])
            {
                return (int[])intArray;
            }
            throw new NotImplementedException("Unknown array type");
        }
    }
}
