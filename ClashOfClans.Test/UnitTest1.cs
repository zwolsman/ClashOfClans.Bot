using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClashOfClans.Networking;
using System.IO;
using ClashOfClans.Networking.Packets;
using ClashOfClans.Util;

namespace ClashOfClans.Test
{
    class TestPacket : IPacket
    {
        public Byte Byte = 1;
        public Int16 Int16 = 2;
        public Int24 Int24 = (Int24)3;
        public Int32 Int32 = 4;
        public Int64 Int64 = 5;

        public ushort ID
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void ReadPacket(ClashBinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void WritePacket(ClashBinaryWriter writer)
        {
            writer.Write(Byte);
            writer.WriteBigEndian(Int16);
            writer.Write(Int24);
            writer.WriteBigEndian(Int32);
            writer.WriteBigEndian(Int64);
        }
    }

    [TestClass]
    public class UnitTest1
    {
        TestPacket packet = new TestPacket();
        [TestMethod]
        public void Stream_Test()
        {
            var clashWriter = new ClashBinaryWriter(new MemoryStream());
            packet.WritePacket(clashWriter);
            var clashContent = ((MemoryStream)clashWriter.BaseStream).ToArray();

            var packetWriter = new PacketWriter(new MemoryStream());
            WritePacket(packetWriter, packet);
            var packetContent = ((MemoryStream)packetWriter.BaseStream).ToArray();

            CollectionAssert.AreEqual(packetContent, clashContent);
        }

        private void WritePacket(PacketWriter writer, TestPacket packet)
        {
            writer.WriteByte(packet.Byte);
            writer.WriteInt16(packet.Int16);
            writer.WriteInt24((int)packet.Int24);
            writer.WriteInt32(packet.Int32);
            writer.WriteInt64(packet.Int64);

        }
    }
}
