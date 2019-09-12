using System.ComponentModel;
using WpfUniverse.ViewModels;

namespace WpfUniverse.Views
{
    /// <summary>
    ///     Interaction logic for PlanetView.xaml
    /// </summary>
    public partial class PlanetView
    {
        public PlanetView()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
                DataContext = CastleContainer.Container.Resolve<PlanetsViewModel>();
        }
    }
}