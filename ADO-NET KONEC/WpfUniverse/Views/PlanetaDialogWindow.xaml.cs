using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WpfUniverse.Entities;
using WpfUniverse.ViewModels;

namespace WpfUniverse.Views
{
    /// <summary>
    /// Diky implementaci rozhrani ma toto okno metodu Close(), nemusi se psat rucne uz je implementovana.
    /// Interaction logic for PlanetaDialogWindow.xaml
    /// </summary>
    public partial class PlanetaDialogWindow : IDialogWindow
    {
        public PlanetaDialogWindow()
        {
            InitializeComponent();
        }
    }
}
