using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking
{
    public class PacketReader : BinaryReader
    {
        public PacketReader(Stream stream)
            : base(stream)
        {
            // Space
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, 0, count);
        }

        public override byte ReadByte()
        {
            return (byte)BaseStream.ReadByte();
        }

        public override bool ReadBoolean()
        {
            var state = ReadByte();
            switch (state)
            {
                case 1:
                    return true;
                case 0:
                    return false;
                default:
                    throw new Exception("A boolean had an incorrect value: " + state + ".");
            }
        }

        public override short ReadInt16()
        {
            return (short)ReadUInt16();
        }

        public override ushort ReadUInt16()
        {
            var buffer = ReadBytesWithEndian(2);
            return BitConverter.ToUInt16(buffer, 0);
        }

        public int ReadInt24()
        {
            var packetLengthBuffer = ReadBytesWithEndian(3, false);
            return ((packetLengthBuffer[0] << 16) | (packetLengthBuffer[1] << 8)) | packetLengthBuffer[2];
        }

        public uint ReadUInt24()
        {
            return (uint)ReadInt24();
        }

        public override int ReadInt32()
        {
            return (int)ReadUInt32();
        }

        public override uint ReadUInt32()
        {
            var buffer = ReadBytesWithEndian(4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public override long ReadInt64()
        {
            return (long)ReadUInt64();
        }

        public override ulong ReadUInt64()
        {
            var buffer = ReadBytesWithEndian(8);
            return BitConverter.ToUInt64(buffer, 0);
        }

        public byte[] ReadByteArray()
        {
            var length = ReadInt32();
            if (length == -1)
                return null;
            if (length < -1)
                throw new Exception("A byte array length was incorrect: " + length + ".");
            if (length > BaseStream.Length - BaseStream.Position)
                throw new Exception(string.Format("A byte array was larger than remaining bytes. {0} > {1}.", length, BaseStream.Length - BaseStream.Position));
            var buffer = ReadBytesWithEndian(length, false);
            return buffer;
        }

        public override string ReadString()
        {
            var length = ReadInt32();
            if (length == -1)
                return null;
            if (length < -1)
                throw new Exception("A string length was incorrect: " + length);
            if (length > BaseStream.Length - BaseStream.Position)
                throw new Exception(string.Format("A string was larger than remaining bytes. {0} > {1}.", length, BaseStream.Length - BaseStream.Position));
            var buffer = ReadBytesWithEndian(length, false);
            return Encoding.UTF8.GetString(buffer);
        }

        public long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        private byte[] ReadBytesWithEndian(int count, bool switchEndian = true)
        {
            var buffer = new byte[count];
            BaseStream.Read(buffer, 0, count);
            if (BitConverter.IsLittleEndian && switchEndian)
                Array.Reverse(buffer);
            return buffer;
        }
    }
}