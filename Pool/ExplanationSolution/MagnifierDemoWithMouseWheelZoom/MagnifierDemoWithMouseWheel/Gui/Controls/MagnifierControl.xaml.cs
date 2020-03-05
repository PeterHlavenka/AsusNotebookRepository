using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Gui.Controls
{
    /// <summary>
    ///     BitmapImage as ImageSource is requiered
    /// </summary>
    public partial class MagnifierControl
    {
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(int), typeof(MagnifierControl), new PropertyMetadata(120));

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(BitmapImage), typeof(MagnifierControl), new PropertyMetadata());

        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("Zoom", typeof(double), typeof(MagnifierControl), new PropertyMetadata(0.3));

        public static readonly DependencyProperty IsMouseWheelZoomEnabledProperty =
            DependencyProperty.Register("IsMouseWheelZoomEnabled", typeof(bool), typeof(MagnifierControl), new PropertyMetadata(true));


        public MagnifierControl()
        {
            InitializeComponent();

            DataContext = this;
        }        

        /// <summary>
        ///     Requiered value is from 0.0  to 1.0 where zero is mimimum zoom
        /// </summary>
        public double Zoom
        {
            private get { return Math.Abs((double) GetValue(ZoomProperty)); }
            set { SetValue(ZoomProperty, value); }
        }

        public BitmapImage ImageSource
        {
            get => (BitmapImage) GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public int Size
        {
            get => (int) GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public bool IsMouseWheelZoomEnabled
        {
            get => (bool) GetValue(IsMouseWheelZoomEnabledProperty);
            set => SetValue(IsMouseWheelZoomEnabledProperty, value);
        }

        private double Length { get; set; }

        private void ContentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            SetupMagnifier(e);
        }

        private void SetupMagnifier(MouseEventArgs e)
        {
            var center = e.GetPosition(ContentPanel);
            Length = MagnifierCircle.ActualWidth * (1 - Zoom);

            if (Length < 0)
            {
                return;
            }

            var radius = Length / 2;
            var viewboxRect = new Rect(center.X - radius, center.Y - radius, Length, Length);
            MagnifierBrush.Viewbox = viewboxRect;

            MagnifierCircle.SetValue(Canvas.LeftProperty, center.X - MagnifierCircle.ActualWidth / 2);
            MagnifierCircle.SetValue(Canvas.TopProperty, center.Y - MagnifierCircle.ActualHeight / 2);
        }

        private void ContentPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            MagnifierCircle.Visibility = Visibility.Visible;
        }

        private void ContentPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            MagnifierCircle.Visibility = Visibility.Hidden;
        }

        private void ContentPanel_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var value = 0.03;

            if (IsMouseWheelZoomEnabled )
            {
                if (! Keyboard.IsKeyDown(Key.LeftAlt))
                {
                    return;
                }

                if (e.Delta > 0)
                {
                    if (Zoom < 1.0 - value)
                    {
                        Zoom += 0.03;
                    }
                }
                else if (e.Delta < 0)
                {
                    if (Zoom > 0 + value)
                    {
                        Zoom -= 0.03;
                    }
                }

                SetupMagnifier(e);
            }
        }
    }
}