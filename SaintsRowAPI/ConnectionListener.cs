using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;

using SaintsRowAPI.Hydra;

using AaltoTLS;

namespace SaintsRowAPI
{
    public class ConnectionListener
    {
        private Socket ListenSocket;
        public bool ValidIP { get; private set; }

        public ConnectionListener()
        {
            IPHostEntry host_remote = Dns.GetHostEntry("sr3.hydra.agoragames.com");
            IPAddress ip_remote = host_remote.AddressList.First();

            IPHostEntry host_local = Dns.GetHostEntry(IPAddress.Loopback);
            IPAddress[] ip_local_all = host_local.AddressList;

            ValidIP = ip_local_all.Any(local_ip => local_ip.Equals(ip_remote));

            if (ValidIP)
            {
                ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ListenSocket.Bind(new IPEndPoint(ip_remote, 443));

                Console.WriteLine("IP: " + ip_remote.ToString());
            }
            else
            {
                Console.WriteLine("[STOP] sr3.hydra.agoragames.com ({0}) doesn't point to this machine.", ip_remote);
            }
        }
        
        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                Socket clientSocket = ListenSocket.Accept();

                Console.WriteLine("New connection!");

                HydraConnection connection = new HydraConnection(clientSocket);
                ThreadStart ts = new ThreadStart(connection.Handle);
                Thread t = new Thread(ts)
                {
                    IsBackground = false
                };
                t.Start();
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Listen()
        {
            if (!ValidIP)
                return;

            ListenSocket.Listen(10);

            Console.WriteLine("Ready for connections.");

            while (true)
            {
                try
                {
                    Socket clientSocket = ListenSocket.Accept();

                    HydraConnection connection = new HydraConnection(clientSocket);
                    ThreadStart ts = new ThreadStart(connection.Handle);
                    Thread t = new Thread(ts)
                    {
                        IsBackground = false
                    };
                    t.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
