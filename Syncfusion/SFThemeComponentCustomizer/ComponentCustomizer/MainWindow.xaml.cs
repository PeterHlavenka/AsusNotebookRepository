using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Syncfusion.SfSkinManager;
using Syncfusion.Themes.Windows11MintDark.WPF;
using Syncfusion.Themes.Windows11MintLight.WPF;
using SyncfusionLicenceRegistrator;
using SyncfusionThemeRegistrator;

namespace ComponentCustomizer;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private VisualStyles m_visualStyle;
    // private const string ThemeName = "Office2019Colorful_Kite";

    public MainWindow()
    {
        try
        {
            SfLicenceRegistrator.Register();
            SfThemeRegistrator.RegisterTheme(VisualStyles.Windows11Light, "Windows11MintLight");
            SfThemeRegistrator.RegisterTheme(VisualStyles.Windows11Dark, "Windows11MintDark");
            SfThemeRegistrator.SetVisualStyle(this, VisualStyles.Windows11Dark);
           
            InitializeComponent();
            SfThemeRegistrator.SetTheme(this);
            // SfThemeRegistrator.RegisterResources();

            ThemeSelector.ItemsSource = new List<VisualStyles>
            {
                VisualStyles.Windows11Light,
                VisualStyles.Windows11Dark
            };

            PrimaryBackgroundSelector.ItemsSource = new List<string>
            {
                "Mint",
                "Teal"
            };
        
            ComponentSelector.ItemsSource = new List<ComboItem>
            {
                new(new Button()),
                new(new TextBox()),
                new(new UpDown()),
                new(new ListBoxFromSfThemeCustomizerProject()),
                // // new (new CardView()),
                new(new ColorPickerPalette()),
                new(new ToggleButton())
            };


            ComponentSelector.SelectedIndex = ComponentSelector.Items.Count - 1;
        }
        catch (Exception e)
        {
            MessageBox.Show($"Error initializing the application: {e.Message}", "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Component_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox cb) ContentControl.Content = (cb.SelectedItem as ComboItem)?.Control;
    }

    private void Theme_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Primary_OnSelectionChanged(sender, e);
    }

    private void Primary_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PrimaryBackgroundSelector.SelectedItem == null || ThemeSelector.SelectedItem == null) return;

        // Změna PrimaryBackground barvy dark motivu. (Změní se pro oba motivy, protože stejná vlastnost je v obou motivech)
        var color = "#FF00B294";
        var selectedPrimary = PrimaryBackgroundSelector.SelectedItem.ToString();
        var selectedTheme = ThemeSelector.SelectedItem.ToString();

        color = selectedPrimary switch
        {
            "Teal" => "#FF007B66",
            "Mint" => "#FF00B294",
            _ => color
        };

        switch (selectedTheme)
        {
            case "Windows11Light":
            {
                var themeSettings = new Windows11MintLightThemeSettings
                {
                    PrimaryBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)!)
                };
                ChangePrimary(themeSettings, VisualStyles.Windows11Light, "Windows11MintLight");
                break;
            }
            case "Windows11Dark":
            {
                var themeSettings = new Windows11MintDarkThemeSettings
                {
                    PrimaryBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)!)
                };
                ChangePrimary(themeSettings, VisualStyles.Windows11Dark, "Windows11MintDark");
                break;
            }
        }
        void ChangePrimary(IThemeSetting themeSettings, VisualStyles visualStyle, string customName)
        {
            SfSkinManager.RegisterThemeSettings(customName, themeSettings);
            SfThemeRegistrator.RegisterTheme(visualStyle, customName);
            SfThemeRegistrator.SetVisualStyle(this, visualStyle);
            SfThemeRegistrator.SetTheme(this);
        }
    }

    // Upravi theme properties, umí změnit vlastnosti motivu
    // Vlastnosti motivu je lepší měnit v common filech vygenerované theme solution, přebuildit a vyměnit referencovanou dllku v projektu, kde se theme používá.
    private void RegisterThemeSettings()
    {
        IThemeSetting themeSettings;
        string customName;
        
        if (m_visualStyle == VisualStyles.Windows11Light)
        {
            customName = "Windows11Light";
            themeSettings = new Windows11MintLightThemeSettings
            {
                PrimaryBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00B294")!) // DarkCyan (světlejší mint)
            };
            SfSkinManager.RegisterThemeSettings(customName, themeSettings);
        }
        
        if (m_visualStyle == VisualStyles.Windows11Dark)
        {
            customName = "Windows11Dark";
            themeSettings = new Windows11MintDarkThemeSettings
            {
                PrimaryBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF007B66")!) // Teal (tmavší mint)
            };
            SfSkinManager.RegisterThemeSettings(customName, themeSettings);
        }
    }
}