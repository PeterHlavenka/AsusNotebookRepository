using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SvgXamlTest;

public partial class ToggleSvgButton : UserControl
{
    public static readonly DependencyProperty SvgSourceProperty =
        DependencyProperty.Register(nameof(SvgSource), typeof(Uri), typeof(ToggleSvgButton), new PropertyMetadata(null));

    public static readonly DependencyProperty PrimaryColorProperty =
        DependencyProperty.Register(nameof(PrimaryColor), typeof(Brush), typeof(ToggleSvgButton), new PropertyMetadata(Brushes.Green));

    public static readonly DependencyProperty SecondaryColorProperty =
        DependencyProperty.Register(nameof(SecondaryColor), typeof(Brush), typeof(ToggleSvgButton), new PropertyMetadata(Brushes.Gray));

    public static readonly DependencyProperty IsCheckedProperty =
        DependencyProperty.Register(nameof(IsChecked), typeof(bool?), typeof(ToggleSvgButton),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsCheckedChanged));

    public ToggleSvgButton()
    {
        InitializeComponent();

        Toggle.Loaded += (s, e) =>
        {
            var brush = Toggle.IsChecked == true ? PrimaryColor : SecondaryColor;
            UpdateBrush(brush);
        };

        Toggle.Checked += (s, e) => UpdateBrush(PrimaryColor);
        Toggle.Unchecked += (s, e) => UpdateBrush(SecondaryColor);
    }

    public Uri SvgSource
    {
        get => (Uri)GetValue(SvgSourceProperty);
        set => SetValue(SvgSourceProperty, value);
    }

    /// <summary>
    /// Checked state color.
    /// </summary>
    public Brush PrimaryColor
    {
        get => (Brush)GetValue(PrimaryColorProperty);
        set => SetValue(PrimaryColorProperty, value);
    }

    /// <summary>
    /// Unchecked state color.
    /// </summary>
    public Brush SecondaryColor
    {
        get => (Brush)GetValue(SecondaryColorProperty);
        set => SetValue(SecondaryColorProperty, value);
    }

    public bool? IsChecked
    {
        get => (bool?)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ToggleSvgButton control)
        {
            control.Toggle.IsChecked = (bool?)e.NewValue;
            var brush = control.Toggle.IsChecked == true ? control.PrimaryColor : control.SecondaryColor;
            control.UpdateBrush(brush);
        }
    }

    private void UpdateBrush(Brush brush)
    {
        if (SvgIcon?.Drawings is { } group) ChangeFillBrushRecursive(group, brush);
    }

    private static void ChangeFillBrushRecursive(DrawingGroup group, Brush newBrush)
    {
        foreach (var drawing in group.Children)
            switch (drawing)
            {
                case GeometryDrawing geometry:
                    geometry.Brush = newBrush;
                    break;
                case DrawingGroup subgroup:
                    ChangeFillBrushRecursive(subgroup, newBrush);
                    break;
            }
    }
    
    private static void ChangeFillBrushById(DrawingGroup group, string id, Brush newBrush)
    {
        foreach (var drawing in group.Children)
        {
            switch (drawing)
            {
                case GeometryDrawing geometry when geometry.GetValue(NameProperty) as string == id:
                    geometry.Brush = newBrush;
                    break;
                case DrawingGroup subgroup when subgroup.GetValue(NameProperty) as string == id:
                    // If the id is on a group, update all its children
                    ChangeFillBrushById(subgroup, id, newBrush);
                    break;
                case DrawingGroup subgroup:
                    ChangeFillBrushById(subgroup, id, newBrush);
                    break;
            }
        }
    }
}