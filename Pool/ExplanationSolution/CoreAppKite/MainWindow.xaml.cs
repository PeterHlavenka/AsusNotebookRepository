using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
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

namespace CoreAppKite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Worker m_worker;

        public MainWindow()
        {
            InitializeComponent();
            m_worker = new Worker(null);
        }

        // testPipe is the named pipe name. This is the identifier for your pipe.
        // PipeDirection.Out means that it only sends out messages and not receiving
        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await m_worker.Execute();
        }
    }
}