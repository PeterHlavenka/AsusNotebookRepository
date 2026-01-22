#region

using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Drawing.Color;

#endregion

namespace ColorsComparer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ComboColor.ItemsSource = typeof(Colors).GetProperties();
            ComboColor1.ItemsSource = typeof(Colors).GetProperties();
            var test = TryFindResource("RibbonBrushes");
        }

        private void LeftBrushApply(object sender, RoutedEventArgs e)
        {
            ComboColor.SelectedItem = ComboColor.ItemsSource.Cast<PropertyInfo>().FirstOrDefault(d => d.GetValue(d).ToString() == LeftBrushTextBox.Text);
        }

        private void LeftIntApply(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(LeftIntTextBox.Text, out var result))
            {
                var color = Color.FromArgb(result);

                R1.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));

                ComboColor.SelectedItem = ComboColor.ItemsSource.Cast<PropertyInfo>().FirstOrDefault(d => d.GetValue(d).ToString() == R1.Fill.ToString());
            }
        }

        private void ComboColor_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboColor.SelectedItem is PropertyInfo info) R1.Fill = new SolidColorBrush((System.Windows.Media.Color) info.GetValue(ComboColor.SelectedItem));
        }


        private void RightBrushApply(object sender, RoutedEventArgs e)
        {
            ComboColor1.SelectedItem = ComboColor1.ItemsSource.Cast<PropertyInfo>().FirstOrDefault(d => d.GetValue(d).ToString() == MiddleBrushTextBox.Text);
        }

        private void RightIntApply(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MiddleIntTextBox.Text, out var result))
            {
                var color = Color.FromArgb(result);

                R2.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));

                ComboColor1.SelectedItem = ComboColor1.ItemsSource.Cast<PropertyInfo>().FirstOrDefault(d => d.GetValue(d).ToString() == R1.Fill.ToString());
            }
        }

        private void ComboColor1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboColor1.SelectedItem is PropertyInfo info) R2.Fill = new SolidColorBrush((System.Windows.Media.Color) info.GetValue(ComboColor1.SelectedItem));
        }

        private void ARGB_Apply(object sender, RoutedEventArgs e)
        {
            // seru na try
            var A = byte.Parse(A_TextBox.Text);
            var R = byte.Parse(R_TextBox.Text);
            var G = byte.Parse(G_TextBox.Text);
            var B = byte.Parse(B_TextBox.Text);

            R1.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(A, R, G, B ));
        }

        private void ARGB_LeftApply(object sender, RoutedEventArgs e)
        {
            // seru na try
            var A = byte.Parse(A_LeftTextBox.Text);
            var R = byte.Parse(R_LeftTextBox.Text);
            var G = byte.Parse(G_LeftTextBox.Text);
            var B = byte.Parse(B_LeftTextBox.Text);

            R2.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(A, R, G, B));
        }
    }
}