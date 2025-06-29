using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media;
using SharpVectors.Renderers.Wpf;

namespace SvgXamlTest;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


// BUTTON
    // private void SvgButton_Click(object sender, RoutedEventArgs e)
    // {
    //     if (SvgIcon.Drawings is { } drawingGroup)
    //     {
    //         ChangeFillBrushRecursive(drawingGroup, Brushes.Red); // Change to any brush
    //     }
    // }
    //
    // private void ChangeFillBrushRecursive(DrawingGroup group, Brush newBrush)
    // {
    //     foreach (Drawing drawing in group.Children)
    //     {
    //         switch (drawing)
    //         {
    //             case GeometryDrawing geometryDrawing:
    //                 geometryDrawing.Brush = newBrush;
    //                 break;
    //             case DrawingGroup childGroup:
    //                 ChangeFillBrushRecursive(childGroup, newBrush);
    //                 break;
    //         }
    //     }
    // }
    
    
    // TOGGLE BUTTON
    private void SvgToggle_Checked(object sender, RoutedEventArgs e)
    {
        if (SvgIcon.Drawings is { } drawingGroup)
        {
            ChangeFillBrushRecursive(drawingGroup, Brushes.Green);
        }
    }

    private void SvgToggle_Unchecked(object sender, RoutedEventArgs e)
    {
        if (SvgIcon.Drawings is { } drawingGroup)
        {
            ChangeFillBrushRecursive(drawingGroup, Brushes.Gray);
        }
    }

    private void ChangeFillBrushRecursive(DrawingGroup group, Brush newBrush)
    {
        foreach (Drawing drawing in group.Children)
        {
            switch (drawing)
            {
                case GeometryDrawing geometryDrawing:
                    geometryDrawing.Brush = newBrush;
                    break;
                case DrawingGroup childGroup:
                    ChangeFillBrushRecursive(childGroup, newBrush);
                    break;
            }
        }
    }

}