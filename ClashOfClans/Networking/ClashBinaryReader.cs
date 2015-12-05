using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClashOfClans.Util;

namespace ClashOfClans.Networking
{
    public sealed class ClashBinaryReader : BinaryReader
    {
        public ClashBinaryReader() : base(new MemoryStream()) { }
        public ClashBinaryReader(Stream stream) : base(stream) { }

        public override string ReadString()
        {
            var buffer = ReadByteArray();
            return buffer == null
                ? null
                : Encoding.UTF8.GetString(buffer);
        }

        public byte[] ReadByteArray()
        {
            var length = this.ReadInt32BigEndian();
            if (length == -1)
                return null;

            var buffer = new byte[length];
            Read(buffer, 0, buffer.Length);

            return buffer;
        }
    }
}
