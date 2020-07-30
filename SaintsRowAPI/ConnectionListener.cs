using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;

using SaintsRowAPI.Hydra;

using AaltoTLS;
using System.Net.NetworkInformation;

namespace SaintsRowAPI
{
    public class ConnectionListener
    {
        private Socket ListenSocket;
        private bool ValidIP;
        public bool IsConnected { get; private set; }

        public ConnectionListener()
        {
            IPHostEntry host_remote = Dns.GetHostEntry("sr3.hydra.agoragames.com");
            IPAddress ip_remote = host_remote.AddressList.First();

            IPHostEntry host_local = Dns.GetHostEntry(IPAddress.Loopback);
            IPAddress[] ip_local_all = host_local.AddressList;

            ValidIP = ip_local_all.Any(local_ip => local_ip.Equals(ip_remote));
            
            //https://stackoverflow.com/a/50577106/3930332
            IPEndPoint[] ipEndPoints = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();
            bool port_443_available = !ipEndPoints.Any(active_port => active_port.Port == 443);

            if (ValidIP && port_443_available)
            {
                ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    ListenSocket.Bind(new IPEndPoint(ip_remote, 443));
                    Console.WriteLine("Connected IP: " + ip_remote.ToString());
                }
                catch (SocketException sex)
                {
                    Console.WriteLine("Can't open port: {0}", sex.Message);
                }
                
            }

            if (!ValidIP)
                Console.WriteLine("[ERROR] sr3.hydra.agoragames.com ({0}) doesn't point to this machine.", ip_remote);
            
            if (!port_443_available)
                Console.WriteLine("[ERROR] Port 443 is in use.");
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
            if (!IsConnected)
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
