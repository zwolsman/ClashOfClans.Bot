using System;
using System.IO;

namespace ClashOfClans.Util
{
    public static class BinaryReaderExtensions
    {
        public static short ReadInt16BigEndian(this BinaryReader reader)
            => BitConverter.IsLittleEndian
                ? BitConverter.ToInt16(reader.ReadBytesRequired(sizeof (short)).Reversed(), 0)
                : reader.ReadInt16();

        public static ushort ReadUInt16BigEndian(this BinaryReader reader)
            => BitConverter.IsLittleEndian
                ? BitConverter.ToUInt16(reader.ReadBytesRequired(sizeof (ushort)).Reversed(), 0)
                : reader.ReadUInt16();

        public static Int24 ReadInt24BigEndian(this BinaryReader reader)
            => Int24.FromBytes(reader.ReadBytesRequired(Int24.SIZE));

        public static UInt24 ReadUInt24BigEndian(this BinaryReader reader)
            => UInt24.FromBytes(reader.ReadBytesRequired(UInt24.SIZE));

        public static Int24 ReadInt24(this BinaryReader reader)
            => Int24.FromBytes(reader.ReadBytesRequired(Int24.SIZE).Reversed());

        public static UInt24 ReadUInt24(this BinaryReader reader)
            => UInt24.FromBytes(reader.ReadBytesRequired(UInt24.SIZE).Reversed());

        public static int ReadInt32BigEndian(this BinaryReader reader)
            => BitConverter.IsLittleEndian
                ? BitConverter.ToInt32(reader.ReadBytesRequired(sizeof (int)).Reversed(), 0)
                : reader.ReadInt32BigEndian();

        public static uint ReadUInt32BigEndian(this BinaryReader reader)
            => BitConverter.IsLittleEndian
                ? BitConverter.ToUInt32(reader.ReadBytesRequired(sizeof (int)).Reversed(), 0)
                : reader.ReadUInt32();

        public static long ReadInt64BigEndian(this BinaryReader reader)
            => BitConverter.IsLittleEndian
                ? BitConverter.ToInt64(reader.ReadBytesRequired(sizeof (long)).Reversed(), 0)
                : reader.ReadInt64BigEndian();

        public static ulong ReadUInt64BigEndian(this BinaryReader reader)
            => BitConverter.IsLittleEndian
                ? BitConverter.ToUInt64(reader.ReadBytesRequired(sizeof (long)).Reversed(), 0)
                : reader.ReadUInt64();

        public static long Seek(this BinaryReader reader, long offset, SeekOrigin origin)
            => reader.BaseStream.Seek(offset, origin);

        private static byte[] ReadBytesRequired(this BinaryReader reader, int byteCount)
        {
            var result = reader.ReadBytes(byteCount);

            if (result.Length != byteCount)
                throw new EndOfStreamException(string.Format("{0} bytes required from stream, but only {1} returned.",
                    byteCount, result.Length));

            return result;
        }
    }
}