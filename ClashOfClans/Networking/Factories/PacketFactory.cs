using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClashOfClans.Networking.Packets;
using log4net;

namespace ClashOfClans.Networking.Factories
{
    public static class PacketFactory
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof (PacketFactory));

        private static Dictionary<ushort, Type> packetDictionary = new Dictionary<ushort, Type>();

        static PacketFactory()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!typeof (IPacket).IsAssignableFrom(t))
                    continue;
                if (t.IsAbstract)
                    continue;
                IPacket instance = (IPacket) Activator.CreateInstance(t);

                packetDictionary.Add(instance.ID, t);
                logger.DebugFormat("Packet handler with id: 0x{0} ({1})", instance.ID.ToString("X2"), t.Name);
            }
        }

        public static IPacket Create(ushort id)
        {
            Type packetType;

            if (!packetDictionary.TryGetValue(id, out packetType))
            {
                return new UnknownPacket();
            }

            return (IPacket)Activator.CreateInstance(packetType);
        }

        public static bool TryCreate(ushort id, IPacket packet)
        {
            Type packetType;

            if (!packetDictionary.TryGetValue(id, out packetType))
            {
                packet = null;
                return false;
            }

            packet = (IPacket)Activator.CreateInstance(packetType);
            return true;
        }
    }
}