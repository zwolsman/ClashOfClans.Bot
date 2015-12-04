using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClashOfClans.Networking.Factories;
using ClashOfClans.Networking.Packets;
using log4net;
using ClashOfClans.Util;

namespace ClashOfClans.Networking
{
    public class NetworkManager
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(NetworkManager));
        private readonly Pool<SocketAsyncEventArgs> _socketArgsPool ;
        
        private Crypto Crypto { get; }
        
        private PacketBufferManager BufferManager { get; set; }
        private NetworkManagerSettings Settings { get; set; }
        public int Seed { get; set; }

        public Socket Connection { get; }
        public NetworkManager(Socket connection)
        {
            if (connection == null)
            {
                throw new ArgumentException(nameof(connection));
            }

            Func<SocketAsyncEventArgs> generator = () =>                                            // if there isn't a item in the back this will be called -> the generator for the events
            {
                var args = new SocketAsyncEventArgs();

                args.Completed += AsyncOperationCompleted;
                BufferManager.SetBuffer(args);                                                      // <-- don't understand yet marvinn stuf
                PacketToken.Create(args);

                return args;
            };

            _socketArgsPool = new Pool<SocketAsyncEventArgs>(generator);

            Connection = connection;

            Crypto = new Crypto();
            Settings = new NetworkManagerSettings();
            
            SetAsyncOperationPools();

            StartReceive(_socketArgsPool.Get());
        }

        private void SetAsyncOperationPools()
        {
            BufferManager = new PacketBufferManager(Settings.ReceiveOperationCount, Settings.SendOperationCount,
                Settings.BufferSize);
        }

        private void StartReceive(SocketAsyncEventArgs args)
        {
            if (!Connection.ReceiveAsync(args))
                AsyncOperationCompleted(Connection, args);
        }

        private void AsyncOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError != SocketError.Success)
            {
                //TODO handle it
                _logger.Error("Socket disconnected!");
                return;
            }

            if (args.LastOperation == SocketAsyncOperation.Receive)
            {
                IPacket[] packets = ProcessRecievePackets(args);

                if (packets == null)
                {
                    return;
                }

                foreach (IPacket packet in packets)
                {
                    OnPacketReceived(new PacketReceivedEventArgs(packet, null));
                }
            }
            else
            {
                _logger.Debug("Need to handle other operation.");
            }
        }

        private IPacket[] ProcessRecievePackets(SocketAsyncEventArgs args)
        {
            List<IPacket> packetList = new List<IPacket>();
            PacketToken packetToken = args.UserToken as PacketToken;
            int bytesToProcess = args.BytesTransferred;

            if (bytesToProcess == 0)
            {
                _logger.Error("Socket disconnected!");
            }
            ReadPacket:

            //Read the header
            if (packetToken.HeaderReceiveOffset != PacketExtractor.HEADER_SIZE) 
            {
                if (PacketExtractor.HEADER_SIZE > bytesToProcess) //Not enough bytes for the header..
                {
                    _logger.DebugFormat("[Net:ID {0}] Not enough bytes to read header.", packetToken.TokenID);
                    Buffer.BlockCopy(args.Buffer, packetToken.ReceiveOffset, packetToken.Header,
                        packetToken.HeaderReceiveOffset, bytesToProcess);
                    packetToken.HeaderReceiveOffset += bytesToProcess;
                    packetToken.ReceiveOffset = args.Offset;
                    StartReceive(args);
                    return packetList.ToArray();
                }
                else //Received header
                {
                    Buffer.BlockCopy(args.Buffer, packetToken.ReceiveOffset, packetToken.Header, packetToken.HeaderReceiveOffset, PacketExtractor.HEADER_SIZE);
                    packetToken.HeaderReceiveOffset += PacketExtractor.HEADER_SIZE;
                    packetToken.ReceiveOffset += PacketExtractor.HEADER_SIZE;
                    bytesToProcess -= PacketExtractor.HEADER_SIZE;
                    ProcessToken(packetToken);
                }
            }

            //Read the body
            if (packetToken.BodyReceiveOffset != packetToken.Length)
            {
                if (packetToken.Length - packetToken.BodyReceiveOffset > bytesToProcess) //Not enough bytes for the body..
                {
                    _logger.DebugFormat("[Net:ID {0}] Not enough bytes to read body.", packetToken.TokenID);

                    Buffer.BlockCopy(args.Buffer, packetToken.ReceiveOffset, packetToken.Body, packetToken.BodyReceiveOffset, bytesToProcess);
                    packetToken.BodyReceiveOffset += bytesToProcess;
                    packetToken.ReceiveOffset = args.Offset;
                    StartReceive(args);
                    return packetList.ToArray();
                }
                else //Received body
                {
                    Buffer.BlockCopy(args.Buffer, packetToken.ReceiveOffset, packetToken.Body, packetToken.BodyReceiveOffset, packetToken.Length - packetToken.BodyReceiveOffset);
                    bytesToProcess -= packetToken.Length - packetToken.BodyReceiveOffset;
                    packetToken.ReceiveOffset += packetToken.Length - packetToken.BodyReceiveOffset;
                    packetToken.BodyReceiveOffset += packetToken.Length;
                }
            }

            IPacket packet = PacketFactory.Create(packetToken.ID);
            byte[] packetDecryptedData = (byte[]) packetToken.Body.Clone();
            Crypto.Decrypt(packetDecryptedData);

            if (packet is UnknownPacket)
            {
                packet = new UnknownPacket()
                {
                    ID = packetToken.ID,
                    Length = packetToken.Length,
                    Version = packetToken.Version,
                    EncryptedData = packetToken.Body,
                    DecryptedData = packetDecryptedData
                };
            }


            //Read the packet.
            using (ClashBinaryReader reader = new ClashBinaryReader(new MemoryStream(packetDecryptedData)))
            {
                try
                {
                    if (!(packet is UnknownPacket))
                        packet.ReadPacket(reader);
                }
                catch (Exception ex)
                {
                    OnPacketReceived(new PacketReceivedEventArgs(packet, ex));
                    packetToken.Reset();
                    if (bytesToProcess != 0)
                        goto ReadPacket;
                }
            }
            if (packet is UpdateKeyPacket)
            {
                UpdateCiphers(Seed, ((UpdateKeyPacket) packet).Key);
            }
            packetList.Add(packet);
            packetToken.Reset();
            if (bytesToProcess != 0)
            {
                goto ReadPacket;
            }

            packetToken.ReceiveOffset = args.Offset;
            
            StartReceive(args);                                             // just reuse it :D
            return packetList.ToArray();
        }


        private static void ProcessToken(PacketToken token)
        {
            token.ID = (ushort)((token.Header[0] << 8) | (token.Header[1]));
            token.Length = (token.Header[2] << 16) | (token.Header[3] << 8) | (token.Header[4]);
            token.Version = (ushort)((token.Header[5] << 8) | (token.Header[6]));
            token.Body = new byte[token.Length];
        }

        public void SendPacket(IPacket packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            using (var decryptedWriter = new ClashBinaryWriter(new MemoryStream()))
            {
                packet.WritePacket(decryptedWriter);
                var body = ((MemoryStream)decryptedWriter.BaseStream).ToArray();
                Crypto.Encrypt(body);

                if (packet is UpdateKeyPacket)
                    UpdateCiphers(Seed, ((UpdateKeyPacket)packet).Key); // handle update key packet

                using (var encryptedWriter = new ClashBinaryWriter(new MemoryStream())) // write header
                {
                    encryptedWriter.WriteBigEndian((ushort)packet.ID);
                    encryptedWriter.Write((Int24)body.Length);
                    encryptedWriter.WriteBigEndian((ushort)0); // the unknown or the packet version
                    encryptedWriter.Write(body, 0, body.Length);

                    var rawPacket = ((MemoryStream)encryptedWriter.BaseStream).ToArray();

                    // should avoid new objs
                    var args = new SocketAsyncEventArgs();
                    args.SetBuffer(rawPacket, 0, rawPacket.Length);
                    if (!Connection.SendAsync(args))
                        AsyncOperationCompleted(Connection, args);
                }
            }
        }

        private void UpdateCiphers(int seed, byte[] key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            _logger.InfoFormat("Updating ciphers. Seed: {0}, key: {1}", seed, key.Aggregate("", (current, b) => current + b.ToString("x2")));
            Crypto.UpdateCiphers((ulong)seed, key);
        }

        #region Events
        public event EventHandler<PacketReceivedEventArgs> PacketReceived;
        protected internal void OnPacketReceived(PacketReceivedEventArgs args)
        {
            PacketReceived?.Invoke(this, args);
        }

        #endregion
    }
}