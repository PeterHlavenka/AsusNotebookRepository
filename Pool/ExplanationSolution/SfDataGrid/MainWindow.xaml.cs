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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Shared;

namespace SfDataGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            SfSkinManager.ApplyStylesOnApplication = true;
            InitializeComponent();
        }

        private void UIElement_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine();
        }

        private void Themecombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // if (ThemeCombobox.SelectedIndex == 0)
            // {
            //     SkinStorage.SetVisualStyle(Window, "Blend");
            // }
            // if (ThemeCombobox.SelectedIndex == 1)
            // {
            //     SkinStorage.SetVisualStyle(Window, "ShinyBlue");
            // }
            // if (ThemeCombobox.SelectedIndex == 2)
            // {
            //     SkinStorage.SetVisualStyle(Window, "ShinyRed");
            // }
        }
    }
}