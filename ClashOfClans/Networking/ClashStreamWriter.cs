using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking
{
    internal sealed class ClashStreamWriter : BinaryWriter
    {
        public ClashStreamWriter(Stream baseStream)
            : base(baseStream)
        {
        }
        
        public void Write(Int24 value)
        {
            var buf = (byte[])value;
            WriteReversed(buf);                     // this game is retarded lol
        }

        public void Write(UInt24 value)
        {
            var buf = (byte[])value;
            WriteReversed(buf);
        }

        public void WriteByteArray(byte[] value)
        {
            if (value == null)
            {
                Write(-1);
            }
            else
            {
                Write(value.Length);
                Write(value);
            }
        }

        public void WriteString(string value)
        {
            if (value == null)
            {
                Write(-1);
            }
            else
            {
                var buffer = Encoding.UTF8.GetBytes(value);
                Write(buffer.Length);
                Write(buffer);
            }
        }

        private void WriteReversed(byte[] buffer)
        {
            Array.Reverse(buffer);

            Write(buffer, 0, buffer.Length);
        }
    }
}
