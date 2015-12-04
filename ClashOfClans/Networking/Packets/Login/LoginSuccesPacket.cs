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

        public void WritePacket(ClashBinaryWriter writer)
        {
            writer.WriteBigEndian((long)UserID);
            writer.WriteBigEndian((long)UserID1);
            writer.Write(UserToken);
            writer.Write(FacebookID);
            writer.Write(GameCenterID);
            writer.WriteBigEndian((int)MajorVersion);
            writer.WriteBigEndian((int)MinorVersion);
            writer.WriteBigEndian((int)RevisionVersion);
            writer.Write(ServerEnvironment);
            writer.WriteBigEndian((int)LoginCount);
            writer.WriteBigEndian((int)(int)PlayTime.TotalSeconds);

            writer.WriteBigEndian((int)Unknown1);

            writer.Write(FacebookAppID);
            writer.Write(DateTimeConverter.ToJavaTimestamp(DateLastPlayed).ToString()); // should round stuff?
            writer.Write(DateTimeConverter.ToJavaTimestamp(DateJoined).ToString());

            writer.WriteBigEndian((int)Unknown2);

            writer.Write(GooglePlusID);
            writer.Write(CountryCode);
        }
    }
}
