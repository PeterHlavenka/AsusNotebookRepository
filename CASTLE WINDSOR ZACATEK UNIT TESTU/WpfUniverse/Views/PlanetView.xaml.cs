using System.Windows.Controls;
using WpfUniverse.ViewModels;

namespace WpfUniverse.Views
{
    /// <summary>
    /// Interaction logic for PlanetView.xaml
    /// </summary>
    public partial class PlanetView
    {
        public PlanetView()
        {
            InitializeComponent();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = CastleContainer.Container.Resolve<PlanetsViewModel>();
            }
          
        }

        
    }
}
