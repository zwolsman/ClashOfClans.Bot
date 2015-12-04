using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClashOfClans.Networking.Packets;

namespace ClashOfClans.Networking
{
    public class PacketReceivedEventArgs : EventArgs
    {
        public PacketReceivedEventArgs(IPacket packet, Exception ex)
        {
            Packet = packet;
            Exception = ex;
        }

        public IPacket Packet { get; private set; }
        public Exception Exception { get; private set; }
    }
}
