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

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt64(UserID);
            writer.WriteString(UserToken);
            writer.WriteInt32(ClientMajorVersion);
            writer.WriteInt32(ClientContentVersion);
            writer.WriteInt32(ClientMinorVersion);
            writer.WriteString(FingerprintHash);

            writer.WriteString(Unknown1);

            writer.WriteString(OpenUDID);
            writer.WriteString(MacAddress);
            writer.WriteString(DeviceModel);
            writer.WriteInt32(LocaleKey);
            writer.WriteString(Language);
            writer.WriteString(AdvertisingGUID);
            writer.WriteString(OSVersion);

            writer.WriteByte(Unknown2);
            writer.WriteString(Unknown3);

            writer.WriteString(AndroidDeviceID);
            writer.WriteString(FacebookDistributionID);
            writer.WriteBoolean(IsAdvertisingTrackingEnabled);
            writer.WriteString(VendorGUID);
            writer.WriteInt32(Seed);
        }
    }
}
