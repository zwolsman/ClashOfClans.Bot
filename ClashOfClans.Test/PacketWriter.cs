using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking
{
    public class PacketWriter : BinaryWriter

    {
        public PacketWriter(Stream baseStream)
            : base(baseStream)
        {
            // Space
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }

        public void WriteByte(byte value)
        {
            BaseStream.WriteByte(value);
        }

        public void WriteBoolean(bool value)
        {
            WriteByte(value == true ? (byte)1 : (byte)0);
        }

        public void WriteInt16(short value)
        {
            WriteUInt16((ushort)value);
        }

        public void WriteUInt16(ushort value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteBytes(buffer);
        }

        public void WriteInt24(int value)
        {
            var buffer = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            Write(buffer, 1, 3);
        }

        public void WriteUInt24(uint value)
        {
            WriteInt24((int)value);
        }

        public void WriteInt32(int value)
        {
            WriteUInt32((uint)value);
        }

        public void WriteUInt32(uint value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteBytes(buffer);
        }

        public void WriteInt64(long value)
        {
            WriteUInt64((ulong)value);
        }

        public void WriteUInt64(ulong value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteBytes(buffer);
        }

        public void WriteByteArray(byte[] value)
        {
            if (value == null) WriteInt32(-1);
            else
            {
                WriteInt32(value.Length);
                WriteBytes(value, false);
            }
        }

        public void WriteString(string value)
        {
            if (value == null) WriteInt32(-1);
            else
            {
                var buffer = Encoding.UTF8.GetBytes(value);
                WriteInt32(buffer.Length);
                WriteBytes(buffer, false);
            }
        }

        public long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        private void WriteBytes(byte[] buffer, bool switchEndian = true)
        {
            if (BitConverter.IsLittleEndian && switchEndian) Array.Reverse(buffer);
            Write(buffer, 0, buffer.Length);
        }
    }
}