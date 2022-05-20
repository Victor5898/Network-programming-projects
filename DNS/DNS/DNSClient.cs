using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace DNS
{
    public class DNSClient
    {
        private IPAddress DNSServer;
        private LookupClient DnsClient;

        public DNSClient()
        {
            DNSServer = IPAddress.Parse("192.168.1.1");
            DnsClient = new LookupClient( );
        }

        public void ResolveIp(string ip)
        {
            try
            {
                var reverseResult = DnsClient.QueryReverseAsync(IPAddress.Parse(ip));

                foreach (var domain in reverseResult.Result.Answers.PtrRecords())
                {
                    Console.WriteLine(domain.RecordType + ": " + domain.PtrDomainName);
                }
            }
            catch (DnsResponseException ex)
            {
                Console.WriteLine("Dns response exception: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            Console.WriteLine();
        }

        public void ResolveDomain(string domain)
        {
            try
            {
                var domainResults = DnsClient.QueryAsync(domain, QueryType.A);
                if (domainResults.Result.Answers.ARecords().Count() > 0)
                {
                    foreach (var ip in domainResults.Result.Answers.ARecords())
                    {
                        Console.WriteLine(ip.Address);
                    }
                }
                else
                {
                    Console.WriteLine("Domain does not exist.");
                }
            }
            catch (DnsResponseException ex)
            {
                Console.WriteLine("Dns response exception: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            Console.WriteLine();
        }

        public void ChangeDNSServer(string ip)
        {
            try
            {
                IPAddress oldDns = DNSServer;
                DNSServer = IPAddress.Parse(ip);
                DnsClient = new LookupClient(DNSServer);

                Console.WriteLine("Dns server " + oldDns.ToString() + " is changed on " + DNSServer.ToString() + ".\n");
            }
            catch(SocketException ex)
            {
                Console.WriteLine("Socket exception: " + ex.Message);
            }
        }
    }
}
