using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ServerObject
    {
        List<ClientObject> clients;
        Socket handler;

        public ServerObject()
        {
            clients = new List<ClientObject>();
        }

        public void AddClient(ClientObject client)
        {
            clients.Add(client);
        }

        public void Listen()
        {
            IPAddress localIpAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(localIpAddress, 5001);

            Socket listener = new Socket(localIpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(endPoint);
            listener.Listen();

            Console.WriteLine("Server started!");
            Console.WriteLine("Waiting for connection...");

            while (true)
            {
                handler = listener.Accept();
                Console.WriteLine("Client connected successfully!");

                ClientObject client = new ClientObject(handler, this);
                Thread clientThread = new Thread(new ThreadStart(client.Handle));
                clientThread.Start();
            }
        }

        public void Broadcast(string message, string clientId, bool isClosed)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(message);

            for(int i = 0; i < clients.Count; i++)
            {
                if(clientId != clients[i].getId())
                {
                    clients[i].getStream().Write(buffer, 0, buffer.Length);
                }
            }
            if(!isClosed)
            {
                SendMessageToItself(message, clientId);
            }
        }

        private void SendMessageToItself(string message, string clientId)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(message);

            for (int i = 0; i < clients.Count; i++)
            {
                if (clientId == clients[i].getId())
                {
                    clients[i].getStream().Write(buffer, 0, buffer.Length);
                }
            }
        }

        public void RemoveConnection(string id)
        {
            ClientObject clientToRemove = clients.FirstOrDefault(c => c.getId() == id);

            if(clientToRemove != null)
            {
                clients.Remove(clientToRemove);
            }
        }

        public void Disconnect()
        {
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

            for(int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }

            Environment.Exit(0);
        }

    }
}
