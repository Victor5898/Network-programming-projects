using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ClientObject
    {
        private string id { get; set; }
        private NetworkStream stream {get; set;}
        private string clientName;
        private Socket socket;
        private ServerObject server;

        public ClientObject(Socket socket, ServerObject server)
        {
            id = Guid.NewGuid().ToString();
            this.socket = socket;
            this.server = server;
            server.AddClient(this);
        }


        public void Handle()
        {
            stream = new NetworkStream(socket);
            string username = GetMessage();

            string message = username + " joined the chat";

            server.Broadcast(message, this.id, false);

            Console.WriteLine(message);

            while(true)
            {
                try
                {
                    //send the message to all users
                    string sendedMessage = GetMessage();
                    message = "[" + username + "] : " + sendedMessage;
                    server.Broadcast(message, this.id, false);
                    Console.WriteLine(message);
                    if (sendedMessage == "quit")
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    //if user disconects
                    message = username + " left the chat";
                    server.Broadcast(message, this.id, true);
                    Console.WriteLine(message);
                    break;
                } 
            }

            server.RemoveConnection(this.id);
            Close();

        }

        private string GetMessage()
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

            return builder.ToString();

        }

        public void Close()
        {
            if (stream != null)
            {
                stream.Close();
            }
            if(socket != null)
            {
                socket.Close();
            }
        }

        public string getId()
        {
            return this.id;
        }

        public NetworkStream getStream()
        {
            return stream;
        }
    }
}
