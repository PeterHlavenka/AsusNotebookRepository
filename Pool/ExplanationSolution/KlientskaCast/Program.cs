using System;
using InBetweenClassLibrary48;

namespace ConsoleApplication111
{
    internal class Program
    {
        public static void Main(string[] args)
            {
                // TcpChannel channel = new TcpChannel();
                // ChannelServices.RegisterChannel(channel, false);

                Console.WriteLine("Vytvarim proxy wellKnown objektu (jinymi slovy: chci connectnout) MyRemoteObject, ktery lezi na portu 8080");
                var obj = (MyRemoteObject)Activator.GetObject(typeof(MyRemoteObject), "tcp://localhost:8080/MyRemoteObject");

                int result = obj.Add(100, 200);
                Console.WriteLine("Result: {0}", result);
                Console.ReadLine();
            }
    }
}