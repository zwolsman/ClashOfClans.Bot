using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClashOfClans.Util;

namespace ClashOfClans.Networking
{
    public sealed class ClashBinaryWriter : BinaryWriter
    {
        public ClashBinaryWriter() : base(new MemoryStream()) { }
        public ClashBinaryWriter(Stream stream) : base(stream) { }

        public override void Write(string value)
        {
            if (value == null)
            {
                BinaryWriterExtensions.WriteBigEndian(this, -1);
            }
            else
            {
                var buffer = Encoding.UTF8.GetBytes(value);
                Write(buffer);
            }
        }

        public override void Write(byte[] value)
        {
            if (value == null)
            {
                BinaryWriterExtensions.WriteBigEndian(this, -1);
            }
            else
            {
                BinaryWriterExtensions.WriteBigEndian(this, value.Length);
                Write(value, 0, value.Length);
            }
        }
    }
}
