using log4net;
using System;
using System.Collections.Generic;

namespace AlbionMarshaller
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

    public enum OperationType
    {
        Request = 0,
        Response = 1
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class OperationHandler : Attribute
    {
        public OperationCodes OpCode { get; set; }
        public OperationType OpType { get; set; }
        public OperationHandler(OperationCodes opCode, OperationType opType = OperationType.Request)
        {
            OpCode = opCode;
            OpType = opType;
        }
    }

    public abstract class BaseEvent
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

        protected static long[] ConvertToLong(object array)
        {
            if (array is byte[])
            {
                return Array.ConvertAll((byte[])array, b => (long)b);
            }
            else if (array is short[])
            {
                return Array.ConvertAll((short[])array, b => (long)b);
            }
            else if (array is int[])
            {
                return Array.ConvertAll((int[])array, b => (long)b);
            }
            else if (array is long[])
            {
                return (long[])array;
            }
            throw new NotImplementedException("Unknown array type");
        }
    }
}
