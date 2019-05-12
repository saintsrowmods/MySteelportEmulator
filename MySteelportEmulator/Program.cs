using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintsRowAPI;

namespace SaintsRowAPI.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("My Steelport Emulator {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(2));
            Console.WriteLine("https://www.saintsrowmods.com/forum/threads/my-steelport-emulator.17361/");
            Console.WriteLine();

            Certificates.Load();
            ConnectionListener Listener = new ConnectionListener();
            Listener.Listen();

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
