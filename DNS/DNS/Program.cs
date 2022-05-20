using DnsClient;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace DNS
{
    class Program
    {
        public static void Main()
        {
            DNSClient client = new();

            string command;
            while (true)
            {
                Console.Write(">> ");
                command = Console.ReadLine().ToLower();
                if (command.StartsWith("resolve"))
                {
                    string argument = command.Substring(8).Trim();
                    IPAddress ipAddress = null; 

                    try
                    {
                        if (IPAddress.TryParse(argument, out ipAddress))
                        {
                            client.ResolveIp(argument);
                        }
                        else
                        {
                            client.ResolveDomain(argument);
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Exception: " + ex.Message + ".\n");
                    }
                }
                else if (command.StartsWith("use dns"))
                {
                    string argument = command.Substring(8).Trim();
                    try
                    {
                        if (Dns.GetHostEntry(argument) != null)
                        {
                            client.ChangeDNSServer(argument);
                        }
                    }
                    catch(SocketException)
                    {
                        Console.WriteLine("Invalid Dns server ip address.\n");
                    }
                    
                }
                else if (command == "quit")
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Command not supported!\n");
                }
            }
        }
    }
}

