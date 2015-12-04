using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking.Packets
{
    public class KeepAliveRequestPacket : IPacket
    {
        public ushort ID => 0x277C;
        public void ReadPacket(ClashBinaryReader reader)
        {
            
        }

        public void WritePacket(ClashBinaryWriter writer)
        {
            
        }
    }
}
