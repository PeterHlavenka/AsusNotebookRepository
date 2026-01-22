using System.Windows.Controls;

namespace ComponentCustomizer;

public partial class ColorPickerPalette : UserControl
{
    public ColorPickerPalette()
    {
        InitializeComponent();
    }

    public override string? ToString()
    {
        return "ColorPickerPalette";
    }
}