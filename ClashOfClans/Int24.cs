using System;

namespace ClashOfClans
{
    public struct Int24
    {
        private int _int;

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
        public static implicit operator Int32(Int24 int24)
            => int24._int;

        public static implicit operator UInt24(Int24 int24)
            => new UInt24((uint)int24._int);

        // for lack of extending BitConverter
        public static unsafe explicit operator byte[](Int24 int24)  
        {
            var buf = new byte[3];

            fixed(byte* p = buf)
            {
                *(p) = *((byte*)(int24._int));
                *(p + 1) = *((byte*)(int24._int + 1));
                *(p + 2) = *((byte*)(int24._int + 2));
            }

            return buf;
        }
    }

    public struct UInt24
    {
        private uint _uint;

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
        public static implicit operator UInt32(UInt24 uint24)
            => uint24._uint;

        public static implicit operator Int24(UInt24 uint24)
            => new Int24((int)uint24._uint);

        // for lack of extending BitConverter
        public static unsafe explicit operator byte[] (UInt24 uint24)
        {
            var buf = new byte[3];

            fixed (byte* p = buf)
            {
                *(p) = *((byte*)(uint24._uint));
                *(p + 1) = *((byte*)(uint24._uint + 1));
                *(p + 2) = *((byte*)(uint24._uint + 2));
            }

            return buf;
        }
    }
}
