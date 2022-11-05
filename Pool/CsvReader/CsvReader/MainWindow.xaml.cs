using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CsvReader;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private Visibility m_addControlVisibility;
    private ImageSource m_buttonImageSource = new BitmapImage(new Uri(".\\Icons\\Ok.png", UriKind.Relative));
    private TrainingView m_trainingView;
    private AddContentView m_addContentView;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    public Visibility AddControlVisibility
    {
        get => m_addControlVisibility;
        set
        {
            if (value == m_addControlVisibility) return;
            m_addControlVisibility = value;
            OnPropertyChanged();
        }
    }

    public ImageSource ButtonImageSource
    {
        get => m_buttonImageSource;
        private set
        {
            if (Equals(value, m_buttonImageSource)) return;
            m_buttonImageSource = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    // Zmeni Image na buttonu a pro AddContentView zavola Save
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (AddControlVisibility == Visibility.Visible) m_addContentView.Save();

        AddControlVisibility = AddControlVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        ButtonImageSource = AddControlVisibility == Visibility.Visible ? 
            new BitmapImage(new Uri(".\\Icons\\Ok.png", UriKind.Relative)) : 
            new BitmapImage(new Uri(".\\Icons\\Plus.png", UriKind.Relative));
    }
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Priradi kontrolky z xamlu do propert zde.
    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is MainWindow mainWindow)
        {
            m_trainingView = mainWindow.TrainingControl;
            m_addContentView = mainWindow.AddContentControl;
        }
    }
}