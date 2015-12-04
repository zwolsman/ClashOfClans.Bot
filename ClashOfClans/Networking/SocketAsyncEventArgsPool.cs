using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Networking
{
    internal sealed class SocketAsyncEventArgsPool : IDisposable
    {

        private readonly object objectLock = new object();
        private bool disposed;

        public int Capacity { get; }
        public Stack<SocketAsyncEventArgs> Pool { get; }
        public int Count => Pool?.Count ?? 0;

        public SocketAsyncEventArgsPool(int capacity)
        {
            Capacity = capacity;
            Pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        public void Push(SocketAsyncEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentException(nameof(args));
            }

            lock (objectLock)
            {
                Pool.Push(args);
            }
        }

        public SocketAsyncEventArgs Pop()
        {
            lock (objectLock)
            {
                return Pool.Pop();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                for (int i = 0; i < Count; i++)
                {
                    var args = Pop();
                    args.Dispose();
                }
                Pool.Clear();
            }
            disposed = true;
        }
    }
}
