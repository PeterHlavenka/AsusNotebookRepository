using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using InBetweenClassLibrary48;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting .NET 4.8 application...");

            var channel = new TcpChannel(8080);
            // var htt = new HttpChannel(9999);

            Console.WriteLine("Registering TcpChannel - posloucham na kanalu 8080");
            ChannelServices.RegisterChannel(channel , false);

            Console.WriteLine("Na tomto kanalu cekam objekty typu MyRemoteObject");
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(MyRemoteObject), "MyRemoteObject", WellKnownObjectMode.Singleton);
            
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}