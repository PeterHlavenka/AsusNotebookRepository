using System;

namespace ConsoleApplication3
{
    public class Worker : MarshalByRefObject
    {
        public void PrintDomain()
        {
            Console.WriteLine("Object is executing in AppDomain \"{0}\"",
                AppDomain.CurrentDomain.FriendlyName);
        }
    }
}