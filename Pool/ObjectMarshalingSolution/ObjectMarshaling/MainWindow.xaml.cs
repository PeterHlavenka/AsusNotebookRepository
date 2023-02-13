using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Remoting;

namespace ObjectMarshaling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Connecting to .NET 4.8 application...");
            RemotingConfiguration.Configure("Client.exe.config", false);

            var remoteObject = (RemoteObject)Activator.GetObject(
                typeof(RemoteObject),
                "tcp://localhost:8080/RemoteObject");

            Console.WriteLine("Sending message to .NET 4.8 application...");
            Console.WriteLine(remoteObject.GetMessage());

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
