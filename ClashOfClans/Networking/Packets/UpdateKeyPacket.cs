using ClashOfClans.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking.Packets
{
    public class UpdateKeyPacket : IPacket
    {
        public ushort ID => 0x4E20;

        public byte[] Key;
        public int ScramblerVersion; // = 1 Encryption version?

        public void ReadPacket(ClashBinaryReader reader)
        {
            Key = reader.ReadByteArray();
            ScramblerVersion = reader.ReadInt32BigEndian();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteByteArray(Key);
            writer.WriteInt32(ScramblerVersion);
        }
    }
}
