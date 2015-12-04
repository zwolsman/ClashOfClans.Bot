using ClashOfClans.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking.Packets
{
    public class LoginSuccessPacket : IPacket
    {
        public ushort ID => 0x4E88;

        public long UserID;
        public long UserID1;
        public string UserToken;
        public string FacebookID;
        public string GameCenterID;
        public int MajorVersion;
        public int MinorVersion;
        public int RevisionVersion;
        public string ServerEnvironment; // could implment Enum here
        public int LoginCount;
        public TimeSpan PlayTime;
        public int Unknown1;
        public string FacebookAppID;
        public DateTime DateLastPlayed;
        public DateTime DateJoined;
        public int Unknown2;
        public string GooglePlusID;
        public string CountryCode;

        public void ReadPacket(ClashBinaryReader reader)
        {
            UserID = reader.ReadInt64BigEndian();
            UserID1 = reader.ReadInt64BigEndian();
            UserToken = reader.ReadString();
            FacebookID = reader.ReadString();
            GameCenterID = reader.ReadString();
            MajorVersion = reader.ReadInt32BigEndian();
            MinorVersion = reader.ReadInt32BigEndian();
            RevisionVersion = reader.ReadInt32BigEndian();
            ServerEnvironment = reader.ReadString();
            LoginCount = reader.ReadInt32BigEndian();
            PlayTime = TimeSpan.FromSeconds(reader.ReadInt32BigEndian());

            Unknown1 = reader.ReadInt32BigEndian();

            FacebookAppID = reader.ReadString();
            DateLastPlayed = DateTimeConverter.FromJavaTimestamp(double.Parse(reader.ReadString()));
            DateJoined = DateTimeConverter.FromJavaTimestamp(double.Parse(reader.ReadString()));

            Unknown2 = reader.ReadInt32BigEndian();

            GooglePlusID = reader.ReadString();
            CountryCode = reader.ReadString();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt64(UserID);
            writer.WriteInt64(UserID1);
            writer.WriteString(UserToken);
            writer.WriteString(FacebookID);
            writer.WriteString(GameCenterID);
            writer.WriteInt32(MajorVersion);
            writer.WriteInt32(MinorVersion);
            writer.WriteInt32(RevisionVersion);
            writer.WriteString(ServerEnvironment);
            writer.WriteInt32(LoginCount);
            writer.WriteInt32((int)PlayTime.TotalSeconds);

            writer.WriteInt32(Unknown1);

            writer.WriteString(FacebookAppID);
            writer.WriteString(DateTimeConverter.ToJavaTimestamp(DateLastPlayed).ToString()); // should round stuff?
            writer.WriteString(DateTimeConverter.ToJavaTimestamp(DateJoined).ToString());

            writer.WriteInt32(Unknown2);

            writer.WriteString(GooglePlusID);
            writer.WriteString(CountryCode);
        }
    }
}
