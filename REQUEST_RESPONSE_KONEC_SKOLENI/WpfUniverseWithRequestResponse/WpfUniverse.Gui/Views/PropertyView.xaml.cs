using System.Windows.Controls;

namespace WpfUniverse.Gui.Views
{
    /// <summary>
    /// Interaction logic for PropertiesView.xaml
    /// </summary>
    public partial class PropertiesView : UserControl
    {
        public PropertiesView()
        {
            InitializeComponent();

            //if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            //{
            //    DataContext = CastleContainer.Container.Resolve<PropertiesViewModel>();
            //}
        }
    }
}
