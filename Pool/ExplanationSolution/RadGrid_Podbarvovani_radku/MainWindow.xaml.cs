using System.Linq;

namespace RadGrid_Podbarvovani_radku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            RadGridView.ItemsSource = (from c in Enumerable.Range(0, 100) select c).ToList();
        }
    }
}