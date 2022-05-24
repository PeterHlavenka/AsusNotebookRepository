

namespace Ribbon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // string style = "Office2019Colorful_ribbon";
            // SkinHelper styleInstance = null;
            // var skinHelpterStr = "Syncfusion.Themes." + style + ".WPF." + style + "SkinHelper, Syncfusion.Themes." + style + ".WPF";
            // Type skinHelpterType = Type.GetType(skinHelpterStr);
            // if (skinHelpterType != null)
            //     styleInstance = Activator.CreateInstance(skinHelpterType) as SkinHelper;
            // if (styleInstance != null)
            // {
            //     SfSkinManager.RegisterTheme("Office2019Colorful_ribbon", styleInstance);
            // }
            // SfSkinManager.SetTheme(this, new Theme("Office2019Colorful_ribbon;Office2019Colorful"));  
           // SfSkinManager.SetTheme(Ribbon, new Theme("Office2019Colorful_ribbon;Office2019Colorful"));    // dropdowhButton target type does not match type of element DropDownButton 
              //SfSkinManager.SetVisualStyle(Ribbon, VisualStyles.Office2019Colorful);
        }
    }
}