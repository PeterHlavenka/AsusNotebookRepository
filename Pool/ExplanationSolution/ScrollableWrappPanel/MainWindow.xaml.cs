using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScrollableWrappPanel;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        
        var scrollFactory = new FrameworkElementFactory(typeof(ScrollViewer));
        // scrollFactory.SetValue(MaxWidthProperty, 600d);
        // scrollFactory.SetValue(MaxHeightProperty, 800d);
        scrollFactory.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
        scrollFactory.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
        scrollFactory.SetValue(BackgroundProperty, Brushes.Bisque);

        
        var wrapPanelFactory = new FrameworkElementFactory(typeof(WrapPanel));
        wrapPanelFactory.SetValue(MaxWidthProperty, 600d);
        // wrapPanelFactory.SetValue(MaxHeightProperty, 800d);

        // scrollFactory.AppendChild(wrapPanelFactory);
        // stackFactory.AppendChild(scrollFactory);  // nejde
        // wrapPanelFactory.AppendChild(scrollFactory);

        // ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate{VisualTree = stackFactory};
        // ItemsControlInXaml.ItemsPanel = itemsPanelTemplate;


        // Add items to the ItemsControl using FrameworkElementFactory
        for (var i = 0; i < 200; i++)
        {
            var buttonFactory = new FrameworkElementFactory(typeof(Button));
            buttonFactory.SetValue(ContentProperty, $"Button {i + 1}");
            buttonFactory.SetValue(WidthProperty, 100.0);
            buttonFactory.SetValue(HeightProperty, 30.0);

            wrapPanelFactory.AppendChild(buttonFactory);
        }

        scrollFactory.AppendChild(wrapPanelFactory);
        ItemsControlInXaml.Template = new ControlTemplate { VisualTree = scrollFactory };
    }
}