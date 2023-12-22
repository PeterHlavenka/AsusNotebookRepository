using System.Windows;
using Syncfusion.Licensing;

namespace SfChartFontInReview;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        SyncfusionLicenseProvider.RegisterLicense("NzUzNDc0QDMyMzAyZTMzMmUzMFU1cGpYVCtsc3hialpNMU9yM082a1BhUktzbU1tV252bHJkaFlkVk5rMEE9");
        InitializeComponent();
        
        
    }
}