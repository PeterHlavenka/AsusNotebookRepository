using System.Windows.Controls;
using WpfUniverse.ViewModels;

namespace WpfUniverse.Views
{
    /// <summary>
    /// Interaction logic for PropertyView.xaml
    /// </summary>
    public partial class PropertyView : UserControl
    {
        public PropertyView()
        {
            InitializeComponent();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = CastleContainer.Container.Resolve<PropertiesViewModel>();
            }
        }
    }
}
