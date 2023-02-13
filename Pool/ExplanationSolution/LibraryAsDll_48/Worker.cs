using System;

namespace LibraryAsDll_48
{
    public class Worker : MarshalByRefObject, IAsyncShutdown
    {
        public void PrintDomain()
        {
            Console.WriteLine("Object is executing in AppDomain \"{0}\"",
                AppDomain.CurrentDomain.FriendlyName);
        }

        public event EventHandler HasShutdown;
        public void BeginShutdown()
        {
            throw new NotImplementedException();
        }
    }
}