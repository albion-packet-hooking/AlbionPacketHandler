using AlbionMarshaller.Model;
using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbionMarshaller
{
    public class PacketQueue
    {
        public event System.EventHandler ItemAdded;
        private ConcurrentQueue<RawCapture> _packetQueue = new ConcurrentQueue<RawCapture>();
        
        public void Enqueue(RawCapture packet)
        {
            _packetQueue.Enqueue(packet);
            Task.Run(() => ItemAdded?.Invoke(this, null));
        }

        public RawCapture Dequeue()
        {
            RawCapture packet = null;
            _packetQueue.TryDequeue(out packet);
            return packet;
        }
    }
}
