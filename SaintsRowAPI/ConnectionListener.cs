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

        public ConnectionListener()
        {
            ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ListenSocket.Bind(new IPEndPoint(IPAddress.Loopback, 443));
        }

        public void Listen()
        {
            ListenSocket.Listen(10);

            Console.WriteLine("Ready for connections.");

            while (true)
            {
                try
                {
                    Socket clientSocket = ListenSocket.Accept();

                    HydraConnection connection = new HydraConnection(clientSocket);
                    ThreadStart ts = new ThreadStart(connection.Handle);
                    Thread t = new Thread(ts);
                    t.IsBackground = false;
                    t.Start();
                }
                catch
                {
                }
            }
        }
    }
}
