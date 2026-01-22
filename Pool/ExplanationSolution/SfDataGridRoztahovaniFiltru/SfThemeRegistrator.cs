using System;
using System.Collections.Generic;
using System.Windows;
using Syncfusion.SfSkinManager;

namespace SfDataGridRoztahovaniFiltru;

public static class SfThemeRegistrator
{
    private static Theme s_theme;
    private static VisualStyles s_style = VisualStyles.Windows11Light;
    private static readonly Dictionary<VisualStyles, Theme> s_themes = new();

    public static void RegisterTheme( VisualStyles visualStyle, string customName)
    {
        s_themes.Remove(visualStyle);
        
        // Pokus o načtení vlastní DLL assembly, pokud ještě není načtená
        TryLoadCustomThemeAssembly(customName);
        
        SkinHelper styleInstance = null;
        var skinHelperStr = "Syncfusion.Themes." + customName + ".WPF." + customName + "SkinHelper, Syncfusion.Themes." + customName + ".WPF";
        var skinHelperType = Type.GetType(skinHelperStr);
        if (skinHelperType != null)
            styleInstance = Activator.CreateInstance(skinHelperType) as SkinHelper;
        if (styleInstance != null)
            SfSkinManager.RegisterTheme(customName, styleInstance);

        s_themes.TryAdd(visualStyle, new Theme(customName));
    }
    
    private static void TryLoadCustomThemeAssembly(string themeName)
    {
        try
        {
            var assemblyName = $"Syncfusion.Themes.{themeName}Theme.WPF";
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            
            // Zkusíme několik možných cest
            var possiblePaths = new[]
            {
                System.IO.Path.Combine(baseDir, $"{assemblyName}.dll"),
                $@"D:\AsusNotebookRepository\Syncfusion\{assemblyName}.dll"
            };
            
            foreach (var path in possiblePaths)
            {
                if (System.IO.File.Exists(path))
                {
                    System.Reflection.Assembly.LoadFrom(path);
                    return;
                }
            }
        }
        catch
        {
            // Ignorujeme chyby při načítání
        }
    }

    public static void SetVisualStyle(DependencyObject dependencyObject, VisualStyles visualStyle)
    {
        s_style = !s_themes.ContainsKey(visualStyle) ? s_style : visualStyle;
        s_theme = s_themes.GetValueOrDefault(s_style);
        SfSkinManager.SetVisualStyle(dependencyObject, s_style);
    }

    public static void ApplyStyle(DependencyObject dependencyObject, bool forExport)
    {
        var style = forExport
            ? VisualStyles.Windows11Light
            : IsLightTheme() ? VisualStyles.Windows11Light : VisualStyles.Windows11Dark;
        SfSkinManager.SetVisualStyle(dependencyObject, style);
    }
        
    public static bool IsLightTheme()
    {
        return s_style is VisualStyles.Office2019Colorful or VisualStyles.Windows11Light;
    }

    public static void SetTheme(DependencyObject dependencyObject)
    {
        try
        {
            SfSkinManager.SetTheme(dependencyObject, s_theme);
        }
        catch (ArgumentException e)
        {
                
        }
    }

    public static void RegisterResources(bool login = false)
    {
        if (Application.Current == null)
        {
              
            return;
        }
            
        Application.Current.Resources.MergedDictionaries.Clear();

        if (login)
        {
            AddResourceDictionaries([".WPF;component/common/brushes.xaml", ".WPF;component/mscontrol/button.xaml"], true);
            return;
        }
            
        var syncfusionResources = new[]
        {
            ".WPF;component/common/brushes.xaml",
            ".WPF;component/dockingmanager/dockingmanager.xaml",
            ".WPF;component/ribbon/ribbon.xaml",
            ".WPF;component/colorpickerpalette/colorpickerpalette.xaml",
            ".WPF;component/timespanedit/timespanedit.xaml",
            ".WPF;component/updown/updown.xaml",
            ".WPF;component/mscontrol/datepicker.xaml",
            ".WPF;component/mscontrol/tabcontrol.xaml",
            ".WPF;component/tabcontrolext/tabcontrolext.xaml",
            ".WPF;component/mscontrol/window.xaml",
            ".WPF;component/mscontrol/togglebutton.xaml",
            ".WPF;component/mscontrol/listbox.xaml",
            ".WPF;component/mscontrol/textblock.xaml",
            ".WPF;component/cardview/cardview.xaml",
            ".WPF;component/ComboBoxAdv/ComboBoxAdv.xaml",
            ".WPF;component/mscontrol/button.xaml",
            ".WPF;component/mscontrol/listview.xaml",
            ".WPF;component/mscontrol/treeview.xaml",
            ".WPF;component/sfmaskededit/sfmaskededit.xaml",
            ".WPF;component/sfdatagrid/sfdatagrid.xaml",
            ".WPF;component/sftreegrid/sftreegrid.xaml",
            ".WPF;component/visentio/defaultstyles.xaml"
        };

        AddResourceDictionaries(syncfusionResources, true);
           
    }

    private static void AddResourceDictionaries(string[] resources, bool withPrefix)
    {
        if (withPrefix && !s_themes.ContainsKey(s_style))
        {
                
            return;
        }
            
        var prefix = withPrefix ? "../../Syncfusion.Themes." + s_themes[s_style].ThemeName : string.Empty;
        foreach (var dict in resources)
        {
            var path = withPrefix ? prefix + dict : dict;
            try
            {
                Application.Current.Resources.MergedDictionaries.Add(
                    new ResourceDictionary { Source = new Uri(path, UriKind.RelativeOrAbsolute) });
            }
            catch (Exception ex)
            {
                   
            }
        }
    }
        

}