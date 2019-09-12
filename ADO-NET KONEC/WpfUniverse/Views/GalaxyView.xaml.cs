using System.Windows.Controls;
using WpfUniverse.ViewModels;

namespace WpfUniverse.Views
{ 
    public partial class GalaxyView : UserControl
    {
        public GalaxyView()
        {
            InitializeComponent();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = MainWindow.GalaxyViewModel;

            }


           
        }
    }
}
