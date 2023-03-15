using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using InBetweenClassLibrary48;

namespace ConsoleApplication11
{
   public class Client
    {
        static void Main(string[] args)
        {
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, false);

            MyRemoteObject obj = (MyRemoteObject)Activator.GetObject(
                typeof(MyRemoteObject),
                "tcp://localhost:8080/MyRemoteObject");

            int result = obj.Add(100, 200);
            Console.WriteLine("Result: {0}", result);
        }
    }
}