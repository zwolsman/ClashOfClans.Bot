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

        public void WritePacket(ClashBinaryWriter writer)
        {
            writer.WriteBigEndian((int)(int)FailureReason);
/*            if (Fingerprint != null)
                writer.Write(Fingerprint.ToJson());
            else
                writer.Write(null);*/
            writer.Write(HostName);
            writer.Write(AssetsRootUrl);
            writer.Write(iTunesUrl);
            writer.Write(Unknown1);
            writer.WriteBigEndian((int)RemainingTime);
            writer.Write(Unknown2);
            writer.Write(CompressedFingerprintJson);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            //File.WriteAllBytes("dump", ((MemoryStream)writer.BaseStream).ToArray());
        }
    }
}
