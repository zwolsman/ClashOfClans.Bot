using ClashOfClans.Util;
using System;

namespace ClashOfClans
{
    public struct Int24
    {
        private int _int;

        public const int SIZE = 3;

        public Int24(Int32 int32)
        {
            _int = int32 & 0xFFFFFF;
        }

        public Int24(Int16 int16)
        {
            _int = int16;
        }
        
        // Input Conversion
        public static explicit operator Int24(Int32 v)
            => new Int24(v);

        public static explicit operator Int24(Int16 v)  // explicit because it would convert it to this type when < Int16.MaxValue
            => new Int24(v);


        // Output Conversion
        public static explicit operator Int32(Int24 int24)
            => int24._int;

        public static implicit operator UInt24(Int24 int24)
            => new UInt24((uint)int24._int);

        // for lack of extending BitConverter
        public static explicit operator byte[](Int24 int24)  
            => new []
            {
                (byte)(int24._int & 0xFF),
                (byte)(int24._int >> 8 & 0xFF),
                (byte)(int24._int >> 16 & 0xFF)
            };

        public static Int24 FromBytes(byte[] buf)
        {
            if (buf == null)
                throw new ArgumentNullException(nameof(buf));
            if (buf.Length != 3)
                throw new ArgumentException("Buf's length must be 3", nameof(buf));

            return new Int24(buf[0] | buf[1] << 8 | buf[2] << 16);
        }
    }

    public struct UInt24
    {
        private uint _uint;

        public const int SIZE = 3;

        public UInt24(UInt32 uint32)
        {
            _uint = uint32 & 0xFFFFFF;
        }

        public UInt24(UInt16 uint16)
        {
            _uint = uint16;
        }

        // Input Conversion
        public static explicit operator UInt24(UInt32 v)
            => new UInt24(v);

        public static explicit operator UInt24(UInt16 v)
            => new UInt24(v);


        // Output Conversion
        public static explicit operator UInt32(UInt24 uint24)
            => uint24._uint;

        public static implicit operator Int24(UInt24 uint24)
            => new Int24((int)uint24._uint);

        // for lack of extending BitConverter
        public static explicit operator byte[] (UInt24 uint24)
        {
            var buffer = new[]
            {
                (byte)(uint24._uint & 0xFF),
                (byte)(uint24._uint >> 8 & 0xFF),
                (byte)(uint24._uint >> 16 & 0xFF)
            };

            return BitConverter.IsLittleEndian
                ? buffer
                : buffer.Reversed();
        }

        public static UInt24 FromBytes(byte[] buf)
        {
            if (buf == null)
                throw new ArgumentNullException(nameof(buf));
            if (buf.Length != 3)
                throw new ArgumentException("Buf's length must be 3", nameof(buf));

            return new UInt24((uint)(buf[0] | buf[1] << 8 | buf[2] << 16));
        }
    }
}
