using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking.Packets
{
    public class KeepAliveResponsePacket : IPacket
    {
        public ushort ID => 0x4E8C;

        public void ReadPacket(ClashBinaryReader reader)
        {

        }

        public void WritePacket(ClashBinaryWriter writer)
        {

        }
    }
}
