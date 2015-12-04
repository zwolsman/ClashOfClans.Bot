using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking
{
    internal class PacketBufferManager
    {

        private readonly int bufferSize;
        private readonly byte[] bufferBlock;
        private int currentIndex;
        private readonly Stack<int> freeIndex = new Stack<int>(); 
        public PacketBufferManager(int receiveCount, int sendCount, int bufferSize)
        {
            this.bufferSize = bufferSize;
            this.bufferBlock = new byte[bufferSize * (receiveCount + sendCount)];
        }

        public void SetBuffer(SocketAsyncEventArgs args)
        {
            if (freeIndex.Count > 0)
            {
                args.SetBuffer(bufferBlock, freeIndex.Pop(), bufferSize);
            }
            else
            {
                args.SetBuffer(bufferBlock, currentIndex, bufferSize);
                currentIndex += bufferSize;
            }
        }

        public void RemoveBuffer(SocketAsyncEventArgs args)
        {
            freeIndex.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}
