using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SvgXamlTest
{
    public partial class ToggleSvgButton : UserControl
    {
        public static readonly DependencyProperty SvgSourceProperty =
            DependencyProperty.Register(nameof(SvgSource), typeof(Uri), typeof(ToggleSvgButton), new PropertyMetadata(null));

        public static readonly DependencyProperty FillCheckedProperty =
            DependencyProperty.Register(nameof(FillChecked), typeof(Brush), typeof(ToggleSvgButton), new PropertyMetadata(Brushes.Green));

        public static readonly DependencyProperty FillUncheckedProperty =
            DependencyProperty.Register(nameof(FillUnchecked), typeof(Brush), typeof(ToggleSvgButton), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(bool?), typeof(ToggleSvgButton),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsCheckedChanged));

        public ToggleSvgButton()
        {
            InitializeComponent();

            Toggle.Checked += (s, e) => UpdateBrush(FillChecked);
            Toggle.Unchecked += (s, e) => UpdateBrush(FillUnchecked);
        }

        public Uri SvgSource
        {
            get => (Uri)GetValue(SvgSourceProperty);
            set => SetValue(SvgSourceProperty, value);
        }

        public Brush FillChecked
        {
            get => (Brush)GetValue(FillCheckedProperty);
            set => SetValue(FillCheckedProperty, value);
        }

        public Brush FillUnchecked
        {
            get => (Brush)GetValue(FillUncheckedProperty);
            set => SetValue(FillUncheckedProperty, value);
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
                var brush = control.Toggle.IsChecked == true ? control.FillChecked : control.FillUnchecked;
                control.UpdateBrush(brush);
            }
        }

        private void UpdateBrush(Brush brush)
        {
            if (SvgIcon?.Drawings is DrawingGroup group)
            {
                ChangeFillBrushRecursive(group, brush);
            }
        }

        private void ChangeFillBrushRecursive(DrawingGroup group, Brush newBrush)
        {
            foreach (var drawing in group.Children)
            {
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
        }
    }
}
