﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking.Packets
{
    public class KeepAliveRequestPacket : IPacket
    {
        public ushort ID => 0x277C;
        public void ReadPacket(PacketReader reader)
        {
            
        }

        public void WritePacket(PacketWriter writer)
        {
            
        }
    }
}