using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UDPChat
{
    class Program
    {
        public static void Main()
        {
            bool isprivate = false;
            Console.Write("Dati adresa serverului: ");
            string serverAddress = Console.ReadLine();
            Console.Write("Dati portul: ");
            string serverPort = Console.ReadLine();

            Server server = new Server(IPAddress.Parse(serverAddress), Int32.Parse(serverPort), IPAddress.Any, 0);
            Thread serverThread = new(() => server.StartServer());
            serverThread.Start();

            Console.WriteLine("Server-ul functioneaza in regim broadcast!");
            Console.WriteLine("Pentru a trimite mesaj in canal privat trimiteti \"p\"");

            Console.Write("Introduceti numele: ");

            string name = Console.ReadLine();

            Client client = new Client(IPAddress.Parse(serverAddress), Int32.Parse(serverPort));

            string message;

            do
            {
                Thread.Sleep(50);
                Console.Write("Message broadcast: ");

                message = Console.ReadLine();

                if(message == "p")
                {
                    Console.WriteLine("Introduceti adresa: ");
                    string address = Console.ReadLine();
                    Console.WriteLine("Introduceti port-ul: ");
                    string port = Console.ReadLine();

                    server.ReceiveOnlyFromAddress = IPAddress.Parse(address);
                    server.ReceiveOnlyFromPort = Int32.Parse(port);

                    server.Refresh();

                    isprivate = true;

                    do
                    {
                        Console.Write("Message private: ");

                        message = Console.ReadLine();

                        client.Send(" (" + name + ") - " + message);
                    }
                    while (message != "e");
                }
                else
                {
                    if(isprivate == true)
                    {
                        server.ReceiveOnlyFromAddress = IPAddress.Any;
                        server.ReceiveOnlyFromPort = 0;

                        server.Refresh();
                    }

                    isprivate = false;

                    client.Send(" (" + name + ") - " + message);
                }
            }
            while (message != "quit");

            client.Close();
        }
    }
}
