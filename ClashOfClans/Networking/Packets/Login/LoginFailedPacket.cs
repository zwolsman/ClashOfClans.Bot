using ClashOfClans.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking.Packets
{
    public class LoginFailedPacket : IPacket
    {
        public enum LoginFailureReason
        {
            OutdatedContent = 7,
            OutdatedVersion = 8,
            Unknown1 = 9,
            Maintenance = 10,
            TemporarilyBanned = 11,
            TakeRest = 12,
            Locked = 13
        };

        public ushort ID => 0x4E87;

        public LoginFailureReason FailureReason;
        public string HostName;
        public string AssetsRootUrl;
        public string iTunesUrl; // market url
        public string Unknown1;
        public int RemainingTime;
        public byte Unknown2;
        public byte[] CompressedFingerprintJson;
        public string Unknown3;
        public string Unknown4;

        public void ReadPacket(ClashBinaryReader reader)
        {
            FailureReason = (LoginFailureReason)reader.ReadInt32BigEndian();
            var fingerprintJson = reader.ReadString();
           /* if (fingerprintJson != null)
                Fingerprint = new Fingerprint(fingerprintJson);*/
            HostName = reader.ReadString();
            AssetsRootUrl = reader.ReadString();
            iTunesUrl = reader.ReadString();
            Unknown1 = reader.ReadString();
            RemainingTime = reader.ReadInt32BigEndian();
            Unknown2 = reader.ReadByte();
            CompressedFingerprintJson = reader.ReadByteArray();
            Unknown3 = reader.ReadString();
            Unknown4 = reader.ReadString();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt32((int)FailureReason);
/*            if (Fingerprint != null)
                writer.WriteString(Fingerprint.ToJson());
            else
                writer.WriteString(null);*/
            writer.WriteString(HostName);
            writer.WriteString(AssetsRootUrl);
            writer.WriteString(iTunesUrl);
            writer.WriteString(Unknown1);
            writer.WriteInt32(RemainingTime);
            writer.WriteByte(Unknown2);
            writer.WriteByteArray(CompressedFingerprintJson);
            writer.WriteString(Unknown3);
            writer.WriteString(Unknown4);
            //File.WriteAllBytes("dump", ((MemoryStream)writer.BaseStream).ToArray());
        }
    }
}
