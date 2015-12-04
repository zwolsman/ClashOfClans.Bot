using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using ClashOfClans.Data.Csv;
using ClashOfClans.Logic;
using ClashOfClans.Properties;
using log4net;
using ClashOfClans.Util;

namespace ClashOfClans.Networking.Packets
{
    public class OwnHomeDataPacket : IPacket
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(OwnHomeDataPacket));

        public ushort ID { get { return 0x5E25; } }

        public TimeSpan LastVisit;
        public int Unknown1;
        public DateTime Timestamp;
        public int Unknown2;
        public long UserID;
        public TimeSpan ShieldDuration;
        public int Unknown3;
        public int Unknown4;
        public bool Compressed;
        public Village Home;
        public Avatar Avatar;
        public int Unknown6;
        public long UserID1;
        public long UserID2;
        public bool Unknown7;
        public long Unknown8;
        public bool Unknown9;
        public long Unknown10;
        public int Unknown11;
        public int AllianceCastleLevel;
        public int AllianceCastleUnitCapacity;
        public int AllianceCastleUnitCount;
        public string FacebookID; // in Avatar object also?
        public int Gems1;
        public int Unknown14;
        public int Unknown15;
        public int Unknown16;
        public int Unknown18;
        public int Unknown17;
        public bool Unknown19;
        public long Unknown20;
        public byte Unknown21;
        public int Unknown22;
        public int Unknown23;
        public int Unknown24;
        public int Unknown25;
        public int Unknown28;
        public int Unknown27;
        public int Unknown26;

        public void ReadPacket(ClashBinaryReader reader)
        {
            var offset = 0x2A;
            LastVisit = TimeSpan.FromSeconds(reader.ReadInt32BigEndian());
            Unknown1 = reader.ReadInt32BigEndian();
            Timestamp = DateTimeConverter.FromUnixTimestamp(reader.ReadInt32BigEndian());
            Unknown2 = reader.ReadInt32BigEndian();
            UserID = reader.ReadInt64BigEndian();
            ShieldDuration = TimeSpan.FromSeconds(reader.ReadInt32BigEndian());
            Unknown3 = reader.ReadInt32BigEndian();
            Unknown4 = reader.ReadInt32BigEndian();
            Compressed = reader.ReadBoolean();
            Home = new Village();
            Home.Read(reader);

            Avatar = new Avatar();
            // Seems like a whole object
            Unknown6 = reader.ReadInt32BigEndian();
            UserID1 = reader.ReadInt64BigEndian();
            UserID2 = reader.ReadInt64BigEndian();
            Avatar.ID = UserID1;

            switch (reader.ReadByte())
            {
                case 0:
                    break;

                case 1:
                    Avatar.Clan = new Clan();
                    Avatar.Clan.ID = reader.ReadInt64BigEndian();
                    Avatar.Clan.Name = reader.ReadString();
                    Avatar.Clan.Badge = reader.ReadInt32BigEndian();
                    reader.ReadInt32BigEndian();
                    Avatar.Clan.Level = reader.ReadInt32BigEndian();
                    offset += 1;
                    break;

                case 2: // clanless but clan castle built?
                    var lel = reader.ReadInt64BigEndian();
                    break;
            }

            if (Unknown7 = reader.ReadBoolean())
                Unknown8 = reader.ReadInt64BigEndian();

            if (Unknown9 = reader.ReadBoolean())
                Unknown10 = reader.ReadInt64BigEndian();

            reader.Seek(offset, SeekOrigin.Current);
            Unknown11 = reader.ReadInt32BigEndian();
            AllianceCastleLevel = reader.ReadInt32BigEndian(); // -1 if not constructed
            AllianceCastleUnitCapacity = reader.ReadInt32BigEndian();
            AllianceCastleUnitCount = reader.ReadInt32BigEndian();
            Avatar.TownHallLevel = reader.ReadInt32BigEndian();
            Avatar.Username = reader.ReadString();
            FacebookID = reader.ReadString();
            Avatar.Level = reader.ReadInt32BigEndian();
            Avatar.Experience = reader.ReadInt32BigEndian();
            Avatar.Gems = reader.ReadInt32BigEndian();
            Gems1 = reader.ReadInt32BigEndian();
            Unknown14 = reader.ReadInt32BigEndian();
            Unknown15 = reader.ReadInt32BigEndian();
            Avatar.Trophies = reader.ReadInt32BigEndian();
            Avatar.AttacksWon = reader.ReadInt32BigEndian();
            Avatar.AttacksLost = reader.ReadInt32BigEndian();
            Avatar.DefensesWon = reader.ReadInt32BigEndian();
            Avatar.DefensesLost = reader.ReadInt32BigEndian();
            Unknown16 = reader.ReadInt32BigEndian();
            Unknown17 = reader.ReadInt32BigEndian();
            Unknown18 = reader.ReadInt32BigEndian();
            if (Unknown19 = reader.ReadBoolean())
                Unknown20 = reader.ReadInt64BigEndian();
            Unknown21 = reader.ReadByte();
            Unknown22 = reader.ReadInt32BigEndian();
            Unknown23 = reader.ReadInt32BigEndian();
            Unknown24 = reader.ReadInt32BigEndian();
            Unknown25 = reader.ReadInt32BigEndian();

            //TODO: Implement those things cause we are not actually storing them.

            CsvTable table = new CsvTable(Resources.resources, true);

            Resource r = new Resource();
            var count1 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count1; i++)
            {
                var id = reader.ReadInt32BigEndian(); // resource id from resources.csv
                var capacity = reader.ReadInt32BigEndian();
                var row = table.Rows[id - 3000000];

                Debug.WriteLine(row.ItemArray[0].ToString());


                logger.InfoFormat("resource id: {0}, max: {1}", id, capacity);
            }

            var count2 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count2; i++)
            {
                var id = reader.ReadInt32BigEndian(); // resource id from resources.csv
                var amount = reader.ReadInt32BigEndian();
                logger.InfoFormat("resource id: {0}, amount: {1}", id, amount);

            }

            var count3 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count3; i++)
            {
                var id = reader.ReadInt32BigEndian(); // unit id from characters.csv
                var amount = reader.ReadInt32BigEndian();
            }

            var count4 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count4; i++)
            {
                var id = reader.ReadInt32BigEndian(); // spell id from spells.csv
                var amount = reader.ReadInt32BigEndian();
            }

            var count5 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count5; i++)
            {
                var id = reader.ReadInt32BigEndian(); // unit id from characters.csv
                var level = reader.ReadInt32BigEndian();
            }

            var count6 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count6; i++)
            {
                var id = reader.ReadInt32BigEndian(); // spell id from spells.csv
                var level = reader.ReadInt32BigEndian();
            }

            var count7 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count7; i++)
            {
                var id = reader.ReadInt32BigEndian(); // hero id from heros.csv
                var level = reader.ReadInt32BigEndian();
            }

            var count8 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count8; i++)
            {
                var id = reader.ReadInt32BigEndian(); // hero id from heros.csv
                var health = reader.ReadInt32BigEndian();
            }

            var count9 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count9; i++)
            {
                var id = reader.ReadInt32BigEndian(); // hero id from heros.csv
                var state = reader.ReadInt32BigEndian();
            }

            var count10 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count10; i++)
            {
                var id = reader.ReadInt32BigEndian(); // unit id from characters.csv
                var amount = reader.ReadInt32BigEndian();
                var level = reader.ReadInt32BigEndian();
            }

            var count11 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count11; i++)
            {
                var id = reader.ReadInt32BigEndian(); // mission id from missions.csv
            }

            var count12 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count12; i++)
            {
                var id = reader.ReadInt32BigEndian(); // achievement id from achievements.csv
            }

            var count13 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count13; i++)
            {
                var id = reader.ReadInt32BigEndian(); // achievement id from achievements.csv
                var progress = reader.ReadInt32BigEndian();
            }

            var count14 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count14; i++)
            {
                var id = reader.ReadInt32BigEndian(); // npc id from npcs.csv
                var stars = reader.ReadInt32BigEndian();
            }

            var count15 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count15; i++)
            {
                var id = reader.ReadInt32BigEndian(); // npc id from npcs.csv
                var gold = reader.ReadInt32BigEndian();
            }

            var count16 = reader.ReadInt32BigEndian();
            for (int i = 0; i < count16; i++)
            {
                var id = reader.ReadInt32BigEndian(); // npc id from npcs.csv
                var elixir = reader.ReadInt32BigEndian();
            }

            Unknown26 = reader.ReadInt32BigEndian();
            Unknown27 = reader.ReadInt32BigEndian();
            Unknown28 = reader.ReadInt32BigEndian();
        }

        public void WritePacket(PacketWriter writer)
        {
            var offset = 0x2A;
          /*  writer.WriteInt32((int)LastVisit.TotalSeconds);
            writer.WriteInt32(Unknown1);
            writer.WriteInt32((int)DateTimeConverter.ToUnixTimestamp(Timestamp));
            writer.WriteInt32(Unknown2);
            writer.WriteInt64(UserID);
            writer.WriteInt32((int)ShieldDuration.TotalSeconds);
            writer.WriteInt32(Unknown3);
            writer.WriteInt32(Unknown4);
            writer.WriteBoolean(Compressed);
            Home.Write(writer);
            writer.WriteInt32(Unknown6);
            writer.WriteInt64(UserID1);
            writer.WriteInt64(UserID2);

            if (Avatar.Clan != null)
            {
                writer.WriteBoolean(true);
                writer.WriteInt64(Avatar.Clan.ID);
                writer.WriteString(Avatar.Clan.Name);
                writer.WriteInt64(Avatar.Clan.Badge);
                writer.WriteInt32(0); // TODO: Make unknown.
                writer.WriteInt64(Avatar.Clan.Level);
                offset += 1;
            }

            writer.WriteBoolean(Unknown7);
            if (Unknown7)
                writer.WriteInt64(Unknown8);
            writer.WriteBoolean(Unknown9);
            if (Unknown9)
                writer.WriteInt64(Unknown10);

            writer.Write(new byte[offset]);
            writer.WriteInt32(Unknown11);
            writer.WriteInt32(AllianceCastleLevel);
            writer.WriteInt32(AllianceCastleUnitCapacity);
            writer.WriteInt32(AllianceCastleUnitCount);
            writer.WriteInt32(Avatar.TownHallLevel);
            writer.WriteString(Avatar.Username);
            writer.WriteString(FacebookID);
            writer.WriteInt32(Unknown14);
            writer.WriteInt32(Unknown15);
            writer.WriteInt32(Avatar.Trophies);
            writer.WriteInt32(Avatar.AttacksWon);
            writer.WriteInt32(Avatar.AttacksLost);
            writer.WriteInt32(Avatar.DefensesWon);
            writer.WriteInt32(Avatar.DefensesLost);
            writer.WriteInt32(Unknown16);
            writer.WriteInt32(Unknown17);
            writer.WriteInt32(Unknown18);
            writer.WriteBoolean(Unknown19);
            if (Unknown19)
                writer.WriteInt64(Unknown20);
            writer.WriteByte(Unknown21);
            writer.WriteInt32(Unknown22);
            writer.WriteInt32(Unknown23);
            writer.WriteInt32(Unknown24);
            writer.WriteInt32(Unknown25);
            //for (int i = 0; i < 15; i++)
            //    writer.WriteInt32(0);*/
        }
    }
}
