using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking
{
    internal sealed class PacketToken
    {
        private static int NextTokenId = 1;


        public ushort ID { get; set; }
        public int Length { get; set; }
        public ushort Version { get; set; }

        public int ReceiveOffset { get; set; }
        public byte[] Header { get; set; }
        public int HeaderReceiveOffset { get; set; }

        public byte[] Body { get; set; }
        public int BodyReceiveOffset { get; set; }


        public int TokenID { get; private set; }

        private PacketToken(SocketAsyncEventArgs args)
        {
            args.UserToken = this;
            Header = new byte[PacketExtractor.HEADER_SIZE];
            ReceiveOffset = args.Offset;
            TokenID = NextTokenId++;
        }

        public void Reset()
        {
            ID = 0;
            Length = 0;
            Version = 0;

            Header = new byte[PacketExtractor.HEADER_SIZE];
            HeaderReceiveOffset = 0;

            Body = null;
            BodyReceiveOffset = 0;
        }

        public static void Create(SocketAsyncEventArgs args)
        {
            args.UserToken = new PacketToken(args);
        }
    }
}
