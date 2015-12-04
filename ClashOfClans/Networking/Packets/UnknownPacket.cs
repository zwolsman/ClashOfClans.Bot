using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking.Packets
{
    class UnknownPacket : IPacket
    {
        public ushort ID { get; set; }
        public void ReadPacket(ClashBinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void WritePacket(PacketWriter writer)
        {
            throw new NotImplementedException();
        }

        public int Length { get; set; }
        public ushort Version { get; set; }

        public byte[] EncryptedData;
        public byte[] DecryptedData;
    }
}
