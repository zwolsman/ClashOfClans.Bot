using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClashOfClans.Networking.Packets;

namespace ClashOfClans
{
    public class KeepAliveManager : IDisposable
    {
        private bool disposed;

        public int Delay { get; set; }
        public DateTime NextKeepAlive { get; private set; }
        public DateTime LastKeepAlive { get; private set; }

        private Thread KeepAliveThread { get; set; }

        private Client Client { get; }
        private bool Running { get; set; }
        public KeepAliveManager(Client client)
        {
            Delay = 5;
            Client = client;
            NextKeepAlive = DateTime.Now;
            LastKeepAlive = DateTime.MaxValue;
            KeepAliveThread = new Thread(UpdateKeepAlives);
        }

        public void Start()
        {
            Running = true;
            KeepAliveThread.Start();
        }

        public void Stop()
        {
            Running = false;
            KeepAliveThread.Abort();   
        }
        private void UpdateKeepAlives()
        {
            try
            {
                while (Running)
                {
                    if (DateTime.Now >= NextKeepAlive)
                    {
                        Client.SendPacket(new KeepAliveRequestPacket());
                        LastKeepAlive = DateTime.Now;
                        NextKeepAlive = DateTime.Now.AddSeconds(Delay);
                    }
                    Thread.Sleep(Delay * 999);
                }
            }
            catch (ThreadAbortException)
            {
                // aborting
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                Stop();
            }

            disposed = true;
        }

        ~KeepAliveManager()
        {
            Dispose(false);
        }
    }
}
