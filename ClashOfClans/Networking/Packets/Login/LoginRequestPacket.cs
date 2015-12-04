using ClashOfClans.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking.Packets
{
    public class LoginRequestPacket : IPacket
    {
        public ushort ID => 0x2775;

        public long UserID;
        public string UserToken;
        public int ClientMajorVersion;
        public int ClientContentVersion;
        public int ClientMinorVersion;
        public string FingerprintHash;

        public string Unknown1;

        public string OpenUDID;
        public string MacAddress;
        public string DeviceModel;
        public int LocaleKey;
        public string Language;
        public string AdvertisingGUID;
        public string OSVersion;

        public byte Unknown2;
        public string Unknown3;

        public bool IsAdvertisingTrackingEnabled;
        public string AndroidDeviceID;
        public string FacebookDistributionID;
        public string VendorGUID;
        public int Seed;

        public void ReadPacket(ClashBinaryReader reader)
        {
            UserID = reader.ReadInt64BigEndian();
            UserToken = reader.ReadString();
            ClientMajorVersion = reader.ReadInt32BigEndian();
            ClientContentVersion = reader.ReadInt32BigEndian();
            ClientMinorVersion = reader.ReadInt32BigEndian();
            FingerprintHash = reader.ReadString();

            Unknown1 = reader.ReadString();

            OpenUDID = reader.ReadString();
            MacAddress = reader.ReadString();
            DeviceModel = reader.ReadString();
            LocaleKey = reader.ReadInt32BigEndian();
            Language = reader.ReadString();
            AdvertisingGUID = reader.ReadString();
            OSVersion = reader.ReadString();

            Unknown2 = reader.ReadByte();
            Unknown3 = reader.ReadString();

            AndroidDeviceID = reader.ReadString();
            FacebookDistributionID = reader.ReadString();
            IsAdvertisingTrackingEnabled = reader.ReadBoolean();
            VendorGUID = reader.ReadString();
            Seed = reader.ReadInt32BigEndian();
        }

        public void WritePacket(ClashBinaryWriter writer)
        {
            writer.WriteBigEndian((long)UserID);
            writer.Write(UserToken);
            writer.WriteBigEndian((int)ClientMajorVersion);
            writer.WriteBigEndian((int)ClientContentVersion);
            writer.WriteBigEndian((int)ClientMinorVersion);
            writer.Write(FingerprintHash);

            writer.Write(Unknown1);

            writer.Write(OpenUDID);
            writer.Write(MacAddress);
            writer.Write(DeviceModel);
            writer.WriteBigEndian((int)LocaleKey);
            writer.Write(Language);
            writer.Write(AdvertisingGUID);
            writer.Write(OSVersion);

            writer.Write(Unknown2);
            writer.Write(Unknown3);

            writer.Write(AndroidDeviceID);
            writer.Write(FacebookDistributionID);
            writer.Write(IsAdvertisingTrackingEnabled);
            writer.Write(VendorGUID);
            writer.WriteBigEndian((int)Seed);
        }
    }
}
