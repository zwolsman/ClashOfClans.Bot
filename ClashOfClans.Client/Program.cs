using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClashOfClans.Networking.Factories;

namespace ClashOfClans.ClientInstance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting up..");
            string server = "gamea.clashofclans.com";
            int port = 9339;
            Client client = new Client();

            client.Connect(new IPEndPoint(Dns.GetHostAddresses(server)[0], port));
            
            Console.WriteLine("Listening");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            
            
            Thread.Sleep(-1);
        }
    }
}
