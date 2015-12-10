using System;
using System.IO;

namespace ClashOfClans.Util
{
    public static class BinaryWriterExtensions
    {
        public static void WriteBigEndian(this BinaryWriter writer, short value)
        {
            var buffer = BitConverter.GetBytes(value);
            ProcessBytes(writer, buffer);
        }

        public static void WriteBigEndian(this BinaryWriter writer, ushort value)
        {
            var buffer = BitConverter.GetBytes(value);
            ProcessBytes(writer, buffer);
        }

        public static void Write(this BinaryWriter writer, Int24 value)
        {
            var buffer = (byte[]) value;
            ProcessBytes(writer, buffer);
        }

        public static void Write(this BinaryWriter writer, UInt24 value)
        {
            var buffer = (byte[]) value;
            ProcessBytes(writer, buffer);
        }

        public static void WriteBigEndian(this BinaryWriter writer, Int24 value)
        {
            var buffer = (byte[]) value;
            ProcessBytes(writer, buffer.Reversed());
        }

        public static void WriteBigEndian(this BinaryWriter writer, UInt24 value)
        {
            var buffer = (byte[]) value;
            ProcessBytes(writer, buffer.Reversed());
        }

        public static void WriteBigEndian(this BinaryWriter writer, int value)
        {
            var buffer = BitConverter.GetBytes(value);
            ProcessBytes(writer, buffer);
        }

        public static void WriteBigEndian(this BinaryWriter writer, uint value)
        {
            var buffer = BitConverter.GetBytes(value);
            ProcessBytes(writer, buffer);
        }

        public static void WriteBigEndian(this BinaryWriter writer, long value)
        {
            var buffer = BitConverter.GetBytes(value);
            ProcessBytes(writer, buffer);
        }

        public static void WriteBigEndian(this BinaryWriter writer, ulong value)
        {
            var buffer = BitConverter.GetBytes(value);
            ProcessBytes(writer, buffer);
        }

        private static void ProcessBytes(BinaryWriter writer, byte[] buffer)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);

            writer.Write(buffer, 0, buffer.Length);
        }
    }
}