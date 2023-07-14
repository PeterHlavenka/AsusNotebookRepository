using System;
using System.Reflection;
using System.Windows;
using Syncfusion.SfSkinManager;

namespace GridControlSample
{
    public static class SfThemeRegistrator
    {
        private static Theme s_theme;
        private static VisualStyles s_visualStyle;
        // private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        public static void RegisterTheme( VisualStyles visualStyle)
        {
            s_visualStyle = visualStyle;
            
            var themeName = visualStyle.ToString();
            var customName = string.Concat(themeName, "_Kite");

            SkinHelper styleInstance = null;
            var skinHelperStr = "Syncfusion.Themes." + customName + ".WPF." + customName + "SkinHelper, Syncfusion.Themes." + customName + ".WPF";
            var skinHelperType = Type.GetType(skinHelperStr);
            if (skinHelperType != null)
                styleInstance = Activator.CreateInstance(skinHelperType) as SkinHelper;
            if (styleInstance != null)
                SfSkinManager.RegisterTheme(customName, styleInstance);

            s_theme = new Theme(customName);
        }

        public static void SetVisualStyle(DependencyObject dependencyObject)
        {
            SfSkinManager.SetVisualStyle(dependencyObject, s_visualStyle);
        }

        public static void SetTheme(DependencyObject dependencyObject)
        {
            try
            {
                SfSkinManager.SetTheme(dependencyObject, s_theme);
            }
            catch (ArgumentException e)
            {
                // s_log.Info(string.Concat(e.Message, $" DependencyObject: {dependencyObject}"));
            }
        }
        
        // Rychla zmena custom theme. Volat z WindMainWindow2View behindu.
        // public static void  RegisterThemeSettings()
        // {
        //     SfSkinManager.RegisterThemeSettings("Office2019Colorful_Kite", new Office2019Colorful_KiteThemeSettings
        //     {
        //         PrimaryBackground = (Brush) Application.Current.Resources["AdwindYellow"],
        //         PrimaryForeground = new SolidColorBrush(Colors.Black)
        //     });
        // }
    }
}