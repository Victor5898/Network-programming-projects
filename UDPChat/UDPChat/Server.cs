using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPChat
{
    class Server
    {
        private EndPoint _receiveFrom;
        private readonly Socket _socket;
        public IPAddress ReceiveOnlyFromAddress { get; set; }
        public int ReceiveOnlyFromPort { get; set; }
        public IPAddress ServerListenAddress { get; set; }
        public int ServerPort { get; set; }

        public Server()
        {
            ReceiveOnlyFromAddress = IPAddress.Any;
            ReceiveOnlyFromPort = 0;
            ServerListenAddress = IPAddress.Parse("127.0.0.1");
            ServerPort = 5000;

            _socket = new Socket(ServerListenAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            _receiveFrom = new IPEndPoint(ReceiveOnlyFromAddress, ReceiveOnlyFromPort);
        }

        public Server(IPAddress address, int port, IPAddress receiveOnlyFromAddress, int receiveOnlyFromPort)
        {
            this.ReceiveOnlyFromAddress = receiveOnlyFromAddress;
            this.ReceiveOnlyFromPort = receiveOnlyFromPort;
            this.ServerListenAddress = address;
            this.ServerPort = port;

            _socket = new Socket(ServerListenAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            _receiveFrom = new IPEndPoint(ReceiveOnlyFromAddress, ReceiveOnlyFromPort);
        }

        public void StartServer()
        {
            try
            {
                _socket.Bind(new IPEndPoint(ServerListenAddress, ServerPort));


                while (true)
                {
                    Receive();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Close();
            }
        }

        public void Receive()
        {
            byte[] buffer = new byte[1024];
            try
            {
                StringBuilder message;
                int bytes;

                message = new StringBuilder();
                bytes = 0;

                do
                {
                    bytes = _socket.ReceiveFrom(buffer, SocketFlags.None, ref _receiveFrom);

                    message.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
                }
                while (_socket.Available > 0);

                IPEndPoint receiveFromIp = _receiveFrom as IPEndPoint;

                Console.WriteLine("\n" + receiveFromIp.Address.ToString() + " : " + receiveFromIp.Port.ToString() + message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void Refresh()
        {
            _receiveFrom = new IPEndPoint(ReceiveOnlyFromAddress, ReceiveOnlyFromPort);
        }

        private void Close()
        {
            if (_socket != null)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }

            Console.WriteLine("Server is shutted down!");

            Environment.Exit(0);
        }
    }
}
