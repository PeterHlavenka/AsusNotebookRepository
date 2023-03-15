using System;
using LibraryAsDll_48;
using Wind.Login;

namespace ConsoleApplication4_48
{
    internal class Program
    {
        private static Isolated<Worker> _isolatedLogin;

        public static void Main(string[] args)
        {
            _isolatedLogin = new Isolated<Worker>("Nejaka domena");
            _isolatedLogin.Object.PrintDomain();
            // m_isolatedLogin.Object.Autostart = true;
            // m_isolatedLogin.Object.Start(m_loginService, configText, PathUtils.DefaultLogsDirectory);

            Console.WriteLine();
            
            _isolatedLogin.Object.Start();
        }
    }
}