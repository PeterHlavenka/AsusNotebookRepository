using System.ComponentModel;
using WpfUniverse.ViewModels;

namespace WpfUniverse.Views
{
    public partial class GalaxyView
    {
        public GalaxyView()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
                DataContext = CastleContainer.Container.Resolve<GalaxyViewModel>();
        }
    }
}