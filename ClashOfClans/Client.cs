using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClashOfClans.Data.Csv;
using ClashOfClans.Networking;
using ClashOfClans.Networking.Packets;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace ClashOfClans
{
    public class Client
    {
        public Socket Connection { get; set; }
        public NetworkManager NetworkManager { get; set; }
        public KeepAliveManager KeepAliveManager { get; set; }
        public bool Connected => Connection?.Connected ?? false;

        private static readonly ILog logger = LogManager.GetLogger(typeof(Client));


        public Client()
        {
#if DEBUG
            var tracer = new TraceAppender();
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.AddAppender(tracer);
            var patternLayout = new PatternLayout { ConversionPattern = "%m%n" };
            tracer.Layout = patternLayout;
            hierarchy.Configured = true;
#endif

            Connection = new Socket(SocketType.Stream, ProtocolType.Tcp);
            KeepAliveManager = new KeepAliveManager(this);
        }

        public void Connect(IPEndPoint endPoint)
        {
            if (endPoint == null)
                throw new ArgumentException(nameof(endPoint));

            var args = new SocketAsyncEventArgs();

            args.Completed += ConnectAsyncCompleted;
            args.RemoteEndPoint = endPoint;

            Connection.ConnectAsync(args);
        }


        private void ConnectAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                throw new SocketException((int)e.SocketError);
            }
            logger.Info($"Connected to {e.RemoteEndPoint}");

            NetworkManager = new NetworkManager(e.ConnectSocket);
            NetworkManager.PacketReceived += NetworkManager_PacketReceived;
            NetworkManager.Seed = new Random().Next();
            NetworkManager.SendPacket(new LoginRequestPacket
            {
                UserID = 356483583763,
                UserToken = "trdan3sxdsgrwpbt29spmy986ww4jbn4zb2jj4rw",
                ClientMajorVersion = 7,
                ClientContentVersion = 0,
                ClientMinorVersion = 200,
                FingerprintHash = "b07b1997df077c85b4863d965aa9f7eb20a0831c",
                OpenUDID = "563a6f060d8624db",
                MacAddress = null,
                DeviceModel = "Nexus 5",
                LocaleKey = 2000005,
                Language = "nl",
                AdvertisingGUID = "",
                OSVersion = "5.1.1",
                IsAdvertisingTrackingEnabled = false,
                AndroidDeviceID = "acfcfd96e80d0a19",
                FacebookDistributionID = "",
                VendorGUID = "",
                Seed = NetworkManager.Seed
            });
            KeepAliveManager.Start();
        }

        private void NetworkManager_PacketReceived(object sender, PacketReceivedEventArgs e)
        {
            logger.InfoFormat("Recieved packet 0x{0} ({1}) {2}", e.Packet.ID.ToString("x2"), e.Packet.ID, e.Packet);


            if (e.Packet is LoginFailedPacket)
            {
                logger.InfoFormat("Failed to log in, reason: {0}", ((LoginFailedPacket)e.Packet).FailureReason);
            }
           /* IPacket packet = e.Packet;
            Debug.WriteLine(packet);*/
        }

        public void SendPacket(IPacket packet)
        {
            NetworkManager.SendPacket(packet);
        }
    }
}
