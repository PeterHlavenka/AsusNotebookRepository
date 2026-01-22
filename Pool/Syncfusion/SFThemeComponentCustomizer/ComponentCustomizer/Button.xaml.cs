using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Adwind.Objects;
using Adwind.UI.Images;

namespace ComponentCustomizer;

public partial class Button : UserControl, INotifyPropertyChanged
{
    private Uri m_svgSource;
    private bool m_showSyntax;

    public Button()
    {
        InitializeComponent();
        DataContext = this;
        
         

        var fileName = "AddGroupArrow.svg";
        var uri = new Uri("pack://application:,,,/Adwind.UI.Images;Component/svgs/" + fileName);
        SvgSource = uri;

        Task.Delay(7000).ContinueWith(_ =>
        {
            fileName = "BeforeEnd.svg";
            uri = new Uri("pack://application:,,,/Adwind.UI.Images;Component/svgs/" + fileName);
            SvgSource = uri;
            ShowSyntax = true; // souvisi s jinym buttonem, ale delay se mi taky hodi
        }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public Uri SvgSource
    {
        get => m_svgSource;
        set
        {
            m_svgSource = value;
            OnPropertyChanged();
        }
    }
    
    public ImageSource ImageSource
    {
        get
        {
            var excludedBrushes = new Brush[]{ Brushes.Gray};
            return ImageLoader.LoadDrawingImage("DetailedAnalysis.svg", Brushes.Maroon, excludedBrushes);
        }
    }

    public bool ShowSyntax
    {
        get => m_showSyntax;
        set
        {
            m_showSyntax = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;


    public override string ToString()
    {
        return "Button";
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        ShowSyntax = !ShowSyntax;
    }
}