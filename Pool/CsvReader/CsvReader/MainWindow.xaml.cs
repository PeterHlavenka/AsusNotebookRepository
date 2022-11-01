using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CsvReader;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private Visibility m_addControlVisibility;
    private ImageSource m_buttonImageSource;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        
        ButtonImageSource = new BitmapImage(new Uri(".\\Icons\\Ok.png", UriKind.Relative));
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
        set
        {
            if (Equals(value, m_buttonImageSource)) return;
            m_buttonImageSource = value;
            OnPropertyChanged();
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        AddControlVisibility = AddControlVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        ButtonImageSource = AddControlVisibility == Visibility.Visible? 
            new BitmapImage(new Uri(".\\Icons\\Ok.png", UriKind.Relative)):
            new BitmapImage(new Uri(".\\Icons\\Plus.png", UriKind.Relative));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}