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
using WpfUniverse.Core;
using WpfUniverse.Entities;
using WpfUniverse.ViewModels;

namespace WpfUniverse.Views
{
    /// <summary>
    /// Interaction logic for GalaxyDialogWindow.xaml
    /// Diky rozhrani se umi zavirat metodou  Close();
    /// </summary>
    public partial class GalaxyDialogWindow : IDialogWindow    
    {
       
        public GalaxyDialogWindow()
        {
            InitializeComponent();

            
        }

    }
}
