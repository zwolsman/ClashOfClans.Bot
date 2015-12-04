namespace ClashOfClans.Networking.Packets
{
    public interface IPacket
    {
        ushort ID { get; }
        void ReadPacket(PacketReader reader);
        void WritePacket(PacketWriter writer);

    }
}