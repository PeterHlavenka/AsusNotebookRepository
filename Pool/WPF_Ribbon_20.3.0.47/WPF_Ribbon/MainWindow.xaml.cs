using Syncfusion.SfSkinManager;

namespace WPF_Ribbon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            SfThemeRegistrator.RegisterTheme(VisualStyles.Office2019Colorful);
           
            InitializeComponent();
           // SfThemeRegistrator.SetVisualStyle(this);

            SfThemeRegistrator.SetTheme(this);          // Not possible: 
             SfThemeRegistrator.SetTheme(Button);
        }
        
    }
}
