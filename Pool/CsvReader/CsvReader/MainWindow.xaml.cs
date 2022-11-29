using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CsvReader;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INotifyPropertyChanged
{
    private AddContentView m_addContentView;
    private Visibility m_addControlVisibility = Visibility.Collapsed;
    private ImageSource m_buttonImageSource = new BitmapImage(new Uri(".\\Icons\\Plus.png", UriKind.Relative));
    private TrainingView m_trainingView;
    private FileInfo m_selectedFileInfo;
    private Visibility m_trainingControlVisibility;
    private bool m_czLanguageChecked;
    private bool m_enLanguageChecked;
    private bool m_deLanguageChecked;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        CreateComboItemsSource();
    }
    public ObservableCollection<FileInfo> FileInfos { get; set; }

    public FileInfo SelectedFileInfo
    {
        get => m_selectedFileInfo;
        set
        {
            m_selectedFileInfo = value;
            if (string.IsNullOrWhiteSpace(m_selectedFileInfo?.Name)) return;

            if (m_trainingView.IsVisible)
                m_trainingView.Initialize(m_selectedFileInfo);

            if (m_addContentView.IsVisible)
                m_addContentView.OpenFile(m_selectedFileInfo);
            
            MainGrid.Focus();
        }
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
    
    public Visibility TrainingControlVisibility
    {
        get => m_trainingControlVisibility;
        set
        {
            if (value == m_trainingControlVisibility) return;
            m_trainingControlVisibility = value;
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

    public bool CzLanguageChecked
    {
        get => m_czLanguageChecked;
        set
        {
            m_czLanguageChecked = value;

            if (m_czLanguageChecked && m_trainingView.AllowedLanguages.All(d => d.Name != LanguageInfo.CzName))
                m_trainingView.AllowedLanguages.Add(new LanguageInfo(){Name = LanguageInfo.CzName});
            else if(m_trainingView.AllowedLanguages.Any(d => d.Name == LanguageInfo.CzName))
                m_trainingView.AllowedLanguages.Remove(m_trainingView.AllowedLanguages.First(d => d.Name == LanguageInfo.CzName));
            
            m_trainingView.InitializeAllowedWords();
            OnPropertyChanged();
            MainGrid.Focus();
        }
    }
    
    public bool EnLanguageChecked
    {
        get => m_enLanguageChecked;
        set
        {
            m_enLanguageChecked = value;

            if (m_enLanguageChecked && m_trainingView.AllowedLanguages.All(d => d.Name != LanguageInfo.EnName))
                m_trainingView.AllowedLanguages.Add(new LanguageInfo(){Name = LanguageInfo.EnName});
            else if(m_trainingView.AllowedLanguages.Any(d => d.Name == LanguageInfo.EnName))
                m_trainingView.AllowedLanguages.Remove(m_trainingView.AllowedLanguages.First(d => d.Name == LanguageInfo.EnName));
            m_trainingView.InitializeAllowedWords();
            MainGrid.Focus();
        }
    }
    
    public bool DeLanguageChecked
    {
        get => m_deLanguageChecked;
        set
        {
            m_deLanguageChecked = value;

            if (m_deLanguageChecked && m_trainingView.AllowedLanguages.All(d => d.Name != LanguageInfo.DeName))
                m_trainingView.AllowedLanguages.Add(new LanguageInfo(){Name = LanguageInfo.DeName});
            else if(m_trainingView.AllowedLanguages.Any(d => d.Name == LanguageInfo.DeName))
                m_trainingView.AllowedLanguages.Remove(m_trainingView.AllowedLanguages.First(d => d.Name == LanguageInfo.DeName));
            m_trainingView.InitializeAllowedWords();
            MainGrid.Focus();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void CreateComboItemsSource()
    {
        var path = Directory.GetCurrentDirectory();
        var directory = new DirectoryInfo(path);
        FileInfos = new ObservableCollection<FileInfo>(
            directory.EnumerateFiles("*.xlsx*", SearchOption.AllDirectories));
    }

    // Zmeni Image na buttonu a pro AddContentView zavola Save
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (AddControlVisibility == Visibility.Visible)
            m_addContentView.Save();
        
        ComboBox.SelectedIndex = -1;

        AddControlVisibility = AddControlVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        TrainingControlVisibility = TrainingControlVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        ButtonImageSource = AddControlVisibility == Visibility.Visible ? 
            new BitmapImage(new Uri(".\\Icons\\Ok.png", UriKind.Relative)) : 
            new BitmapImage(new Uri(".\\Icons\\Plus.png", UriKind.Relative));

        MainGrid.Focus();
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
            CzLanguageChecked = true;
            m_addContentView = mainWindow.AddContentControl;
        }
    }

    private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            m_trainingView.DoNext();
        }
    }
}