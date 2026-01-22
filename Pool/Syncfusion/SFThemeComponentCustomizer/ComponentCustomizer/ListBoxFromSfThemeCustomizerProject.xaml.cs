using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ComponentCustomizer;

/// <summary>
///     Interaction logic for ListBoxFromSfThemeCustomizerProject.xaml
/// </summary>
public partial class ListBoxFromSfThemeCustomizerProject : UserControl
{
    private ListBoxItemWithSvgSource m_selectedItem;

    public ListBoxFromSfThemeCustomizerProject()
    {
        InitializeComponent();

        try
        {
            var svgDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\..\Src\UI\Images\svgs\");
            var svgFiles = Directory.GetFiles(svgDirectory, "*.svg");

            var onlySvgNames = svgFiles.Select(Path.GetFileNameWithoutExtension)!.ToList<string>();
            Origin.ItemsSource = onlySvgNames.Select(str =>
            {
                var formatted = $"/svgs/{str}.svg";
                return new ListBoxItemWithSvgSource(formatted, str, false);
            }).ToList();

            Customized.ItemsSource = onlySvgNames.Select(str =>
            {
                var formatted = $"/svgs/{str}.svg";
                return new ListBoxItemWithSvgSource(formatted, str, true);
            }).ToList();
        }
        catch (Exception e)
        {
            MessageBox.Show($"Error initializing the ListBox: {e.Message}", "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public ListBoxItemWithSvgSource SelectedItem
    {
        get => m_selectedItem;
        set
        {
            if (value == null) return;
            m_selectedItem = value;
            Preview.Source = m_selectedItem.MySource;
            PreviewCustomized.Source = m_selectedItem.MySource;
            
            if (TryFindResource("ContentForeground") is not SolidColorBrush brush) return;
            PreviewCustomized.UpdateBrush(brush);
        }
    }

    public override string? ToString()
    {
        return "ListBox";
    }
}

public class ListBoxItemWithSvgSource(string mySource, string name, bool iscustomized)
{
    public string MySource { get; set; } = mySource;
    public string Name { get; set; } = name;
    
    public bool IsCustomized { get; set; } = iscustomized;
}