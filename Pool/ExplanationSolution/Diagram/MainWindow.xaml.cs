using System.Windows;
using Syncfusion.Licensing;
using Syncfusion.UI.Xaml.Diagram;

namespace Diagram;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NAaF5cWWJCf0x0WmFZfVpgdl9HZ1ZTR2Y/P1ZhSXxQd0dhUX5acXNXRWJVWUM=");
        InitializeComponent();

        //Initialize Nodes with Observable Collection of NodeViewModel.
        diagram.Nodes = new NodeCollection();

        //Initialize Connectors with Observable Collection of ConnectorViewModel
        diagram.Connectors = new ConnectorCollection();
        
        // Nefunguje s ribbonem : 
        // // Creating the NodeViewModel  
        // NodeViewModel Begin = new NodeViewModel()
        // {
        //     ID = "Begin", 
        //     
        //     UnitWidth = 120, 
        //     UnitHeight = 40, 
        //     OffsetX = 300, 
        //     OffsetY = 60,
        //     //Specify shape to the Node from built-in Shape Dictionary
        //     Shape = this.Resources["Ellipse"],
        //     //Apply style to Shape
        //     ShapeStyle = this.Resources["ShapeStyle"] as Style,
        //     Annotations = new AnnotationCollection()
        //     {
        //         new AnnotationEditorViewModel()
        //         {
        //             Content="Begin",
        //         }
        //     }, 
        // };
      
        //Add Node to Nodes property of the Diagram
        // (diagram.Nodes as NodeCollection)?.Add(Begin);
    }
}