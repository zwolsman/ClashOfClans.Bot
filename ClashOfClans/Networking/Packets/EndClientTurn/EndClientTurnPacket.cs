using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking.Packets.EndClientTurn
{
    class EndClientTurnPacket : IPacket
    {
        public ushort ID { get { return 22222; } }
        public void ReadPacket(ClashBinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void WritePacket(ClashBinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
