using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPChat
{
    class Client
    {
        private int ServerPort { get; set; }
        private IPAddress ServerAddress { get; set; }

        private readonly Socket _socket;

        public Client()
        {
            ServerAddress = IPAddress.Parse("127.0.0.1");
            ServerPort = 5000;

            _socket = new Socket(ServerAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        }

        public Client(IPAddress address, int port)
        {
            this.ServerAddress = address;
            this.ServerPort = port;

            _socket = new Socket(ServerAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        }

        public void Send(string message)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);

                _socket.SendTo(buffer, new IPEndPoint(ServerAddress, ServerPort));
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Socket exception: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public void Close()
        {

            if (_socket != null)
            {
                _socket.Close();
            }

            Environment.Exit(0);
        }
    }
}
