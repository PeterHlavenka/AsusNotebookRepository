

using System;
using System.Windows;
using Syncfusion.SfSkinManager;

namespace WPF_Ribbon
{
    public static class SfThemeRegistrator
    {
        private static Theme s_theme;
        private static VisualStyles s_visualStyle;

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
                throw new ApplicationException();
                //s_log.Info(string.Concat(e.Message, $" DependencyObject: {dependencyObject}"));
            }
        }
    }
}