

namespace WpfUniverse
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            new Bootstraper().Start();

            InitializeComponent();
            DataContext = this;
        }

    }
}
