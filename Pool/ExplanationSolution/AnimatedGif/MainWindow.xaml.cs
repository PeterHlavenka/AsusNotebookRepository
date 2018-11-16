using System;
using System.IO;
using System.Windows;

namespace AnimatedGif
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Console.WriteLine(Directory.GetCurrentDirectory());

            Files = Directory.GetFiles(@"\images");  // slozka images je na C
            DataContext = this;
        }

        public string[] Files { get; set; }
    }
}