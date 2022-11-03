using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Tools.Controls;

namespace WPF_Ribbon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            SfThemeRegistrator.RegisterTheme(VisualStyles.Office2019Colorful);
           
            InitializeComponent();
            SfThemeRegistrator.SetVisualStyle(this);
            SfThemeRegistrator.SetTheme(ContentGrid);   //Nejde !!
            // SfThemeRegistrator.SetTheme(this);          //Nejde !!
            // SfThemeRegistrator.SetTheme(Button);
        }
        
    }
}
