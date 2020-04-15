using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static AlbionProcessor.AlbionEvent;

namespace AlbionProcessor
{
    public class PacketHandler
    {
        public readonly string LogTimer = DateTime.UtcNow.ToString("dd-MMM-HH-mm-ss");

        public ConcurrentDictionary<String, int> triggeredEvents = new ConcurrentDictionary<string, int>();
        public ConcurrentDictionary<String, int> triggeredOperations = new ConcurrentDictionary<string, int>();

        private Dictionary<EventCodes, List<HandleEvent>> _eventHandlers = new Dictionary<EventCodes, List<HandleEvent>>();
        private Dictionary<OperationCodes, List<HandleOperation>> _operationHandlers = new Dictionary<OperationCodes, List<HandleOperation>>();

        public PacketHandler()
        {
            var methods = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()
                                                                  .SelectMany(t => t.GetMethods())
                                                                  .Where(m => m.GetCustomAttributes(typeof(EventHandler), false).Length > 0)
                                                                  .ToArray()).ToArray();
            foreach(MethodInfo method in methods)
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
                    if (!_operationHandlers.ContainsKey(opCode))
                    {
                        _operationHandlers.Add(opCode, new List<HandleOperation>());
                    }
                    _operationHandlers[opCode].Add(del);
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

            if (eventCode.ToString().StartsWith("Cast") || eventCode.ToString().StartsWith("Attack") || eventCode.ToString().StartsWith("Health") 
                || eventCode.ToString().Contains("Spell") || eventCode.ToString().Contains("Mount") || eventCode.ToString().Contains("Energy") 
                || eventCode.ToString().Contains("Guild") || eventCode.ToString().Contains("Harvest") || eventCode.ToString().Contains("Channeling"))
            {
                return;
            }

            LogManager.GetLogger("Event." + eventCode.ToString()).Debug(parameters);
            if (_eventHandlers.ContainsKey(eventCode))
            {
                foreach (HandleEvent eventHandler in _eventHandlers[eventCode])
                {
                    eventHandler(parameters, LogManager.GetLogger("Event." + eventCode.ToString()));
                }
            }
        }

        public void OnResponse(byte operationCode, short returnCode, Dictionary<byte, object> parameters)
        {
            int iCode = 0;
            if (int.TryParse(parameters[253].ToString(), out iCode))
            {
                OperationCodes opCode = (OperationCodes)iCode;
                if (opCode.ToString().StartsWith("Move"))
                {
                    return;
                }
                LogManager.GetLogger("Response." + opCode.ToString()).Debug(parameters);
                if (_operationHandlers.ContainsKey(opCode))
                {
                    foreach (HandleOperation opHandler in _operationHandlers[opCode])
                    {
                        opHandler(parameters, LogManager.GetLogger("Response." + opCode.ToString()));
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
                LogManager.GetLogger("Request." + opCode.ToString()).Debug(parameters);
                if (_operationHandlers.ContainsKey(opCode))
                {
                    foreach (HandleOperation opHandler in _operationHandlers[opCode])
                    {
                        opHandler(parameters, LogManager.GetLogger("Request." + opCode.ToString()));
                    }
                }
            }
        }
    }
}
