using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client.Client
{
    class Client
    {
        static string username;
        private const string hostname = "127.0.0.1";
        private const int port = 5001;
        static NetworkStream stream;
        static Socket client;


        public static void Main()
        {
            Console.WriteLine("Introduce your username");
            username = Console.ReadLine();

            IPAddress localIpAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(localIpAddress, 5001);

            client = new Socket(localIpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                client.Connect(endPoint);
                stream = new NetworkStream(client);

                string message = username;
                byte[] buffer = Encoding.Unicode.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                SendMessaage();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ReceiveMessage()
        {
            string message = null;

            while(true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    StringBuilder builder = new StringBuilder();

                    int bytes = 0;

                    do
                    {
                        bytes = stream.Read(buffer, 0, buffer.Length);
                        builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    message = builder.ToString();
                    Console.WriteLine(message);

                    if(message == "[" + username + "] : quit")
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    Console.WriteLine("Connection aborded");
                    Disconnect();
                    break;
                }
            }
        }

        private static void SendMessaage()
        {
            string message;

            Console.WriteLine("Enter the message: ");
            do
            {
                message = Console.ReadLine();
                byte[] buffer = Encoding.Unicode.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
            while (message != "quit");
        }

        private static void Disconnect()
        {
            if(stream != null)
            {
                stream.Close();
            }
            if(client != null)
            {
                client.Close();
            }
        }
    }
}
