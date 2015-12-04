using ClashOfClans.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking.Packets
{
    public class ChatMessageServerPacket : IPacket
    {
        public ushort ID => 0x608B;

        public string Message;
        public string Username;
        public int Level;
        public int League;
        public long UserID;
        public long UserID2;
        public bool HasClan;
        public long ClanID;
        public string ClanName;
        public int Unknown3;

        public void ReadPacket(ClashBinaryReader reader)
        {
            Message = reader.ReadString();
            Username = reader.ReadString();

            Level = reader.ReadInt32BigEndian();
            League = reader.ReadInt32BigEndian();

            UserID = reader.ReadInt64BigEndian();
            UserID2 = reader.ReadInt64BigEndian();
            HasClan = reader.ReadBoolean();
            if (HasClan)
            {
                ClanID = reader.ReadInt64BigEndian();
                ClanName = reader.ReadString();
                Unknown3 = reader.ReadInt32BigEndian();
            }
        }

        public void WritePacket(ClashBinaryWriter writer)
        {
            writer.Write(Message);
            writer.Write(Username);

            writer.WriteBigEndian((int)Level);
            writer.WriteBigEndian((int)League);

            writer.WriteBigEndian((long)UserID);
            writer.WriteBigEndian((long)UserID2);
            writer.Write(HasClan);
            if (HasClan)
            {
                writer.WriteBigEndian((long)ClanID);
                writer.Write(ClanName);
                writer.WriteBigEndian((int)Unknown3);
            }
        }
    }
}
