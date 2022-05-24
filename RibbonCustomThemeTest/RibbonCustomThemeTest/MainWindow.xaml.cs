
using System;
using Syncfusion.SfSkinManager;

namespace RibbonCustomThemeTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow() 
        {
            InitializeComponent();
            
            // SfSkinManager.SetVisualStyle(this, VisualStyles.Office2019White);
            // SfSkinManager.SetVisualStyle(MyRibbon, VisualStyles.Office2019Colorful);
            
             // string style = "MaterialDarkYellow";
             // SkinHelper styleInstance = null;
             // var skinHelpterStr = "Syncfusion.Themes." + style + ".WPF." + style + "SkinHelper, Syncfusion.Themes." + style + ".WPF";
             // Type skinHelpterType = Type.GetType(skinHelpterStr);
             // if (skinHelpterType != null)
             //     styleInstance = Activator.CreateInstance(skinHelpterType) as SkinHelper;
             // if (styleInstance != null)
             // {
             //     SfSkinManager.RegisterTheme("MaterialDarkYellow", styleInstance);
             // }

            // SfSkinManager.SetTheme(this, new Theme("MaterialDarkYellow;MaterialDark"));
            
            
            
            
            
            
            
            // string style = "Office2019Colorful_green";
            // SkinHelper styleInstance = null;
            // var skinHelpterStr = "Syncfusion.Themes." + style + ".WPF." + style + "SkinHelper, Syncfusion.Themes." + style + ".WPF";
            // Type skinHelpterType = Type.GetType(skinHelpterStr);
            // if (skinHelpterType != null)
            //     styleInstance = Activator.CreateInstance(skinHelpterType) as SkinHelper;
            // if (styleInstance != null)
            // {
            //     SfSkinManager.RegisterTheme("Office2019Colorful_green", styleInstance);
            // }
            // SfSkinManager.SetTheme(this, new Theme("Office2019Colorful_green;Office2019Colorful"));
            // SfSkinManager.SetTheme(MyRibbon, new Theme("Office2019Colorful_green;Office2019Colorful"));
        }
    }
}