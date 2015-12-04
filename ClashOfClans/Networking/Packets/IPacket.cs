using System.IO;

namespace ClashOfClans.Networking.Packets
{
    public interface IPacket
    {
        ushort ID { get; }
        void ReadPacket(ClashBinaryReader reader);
        void WritePacket(PacketWriter writer);

    }
}