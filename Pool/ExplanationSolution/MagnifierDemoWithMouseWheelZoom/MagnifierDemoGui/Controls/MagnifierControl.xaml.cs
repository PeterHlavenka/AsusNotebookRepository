using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Gui.Controls
{
    public partial class MagnifierControl
    {
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(int), typeof(MagnifierControl), new PropertyMetadata(120));


        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(BitmapImage), typeof(MagnifierControl), new PropertyMetadata());

        // Using a DependencyProperty as the backing store for Factor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FactorProperty =
            DependencyProperty.Register("Factor", typeof(double), typeof(MagnifierControl), new PropertyMetadata(0.3));


        public static readonly DependencyProperty ContentControlSourceProperty = DependencyProperty.Register(
            "ContentControlSource", typeof(ContentControl), typeof(MagnifierControl), new PropertyMetadata(default(ContentControl)));

        public ContentControl ContentControlSource
        {
            get => (ContentControl) GetValue(ContentControlSourceProperty);
            set => SetValue(ContentControlSourceProperty, value);
        }
        
        public MagnifierControl()
        {
            InitializeComponent();

            DataContext = this;
        }


        public double Factor
        {
            get => (double) GetValue(FactorProperty);
            set => SetValue(FactorProperty, value);
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

        private void ContentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            var center = e.GetPosition(ContentPanel);
            var length = MagnifierCircle.ActualWidth * Factor;
            var radius = length / 2;
            var viewboxRect = new Rect(center.X - radius, center.Y - radius, length, length);
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
    }
}