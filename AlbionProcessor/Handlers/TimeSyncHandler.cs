using AlbionMarshaller;
using AlbionMarshaller.MemoryStorage;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbionProcessor.Handlers
{
    public class TimeSyncHandler : BaseEvent
    {
        [AlbionMarshaller.EventHandler(EventCodes.TimeSync)]
        public static void HandleTimeSync(Dictionary<byte, object> parameters, ILog log)
        {
            WorldDB.Instance.WorldTime = long.Parse(parameters[0].ToString());
        }
    }
}
