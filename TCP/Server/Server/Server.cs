using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

namespace Server
{
    class Server
    {
        static ServerObject server;

        public static void Main(string[] args)
        {
            try
            {
                server = new ServerObject();
                Thread serverThread = new Thread(new ThreadStart(server.Listen));
                serverThread.Start();
            }
            catch(Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

