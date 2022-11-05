using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Syncfusion.Windows.Controls.Grid.Converter;

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
        
        Words = new List<TranslatedObject>();

        for (int i = 1; i < 100; i++)
        {
            Words.Add(new TranslatedObject(i.ToString(), string.Empty, String.Empty, String.Empty));
        }
        
        // DataGrid.ItemsSource = Words;
    }

    public TrainingView TrainingView { get; set; }
    public AddContentView AddContentView { get; set; }

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
        if (AddControlVisibility == Visibility.Visible)
        {
            Save();
        }
        
        AddControlVisibility = AddControlVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        ButtonImageSource = AddControlVisibility == Visibility.Visible? 
            new BitmapImage(new Uri(".\\Icons\\Ok.png", UriKind.Relative)):
            new BitmapImage(new Uri(".\\Icons\\Plus.png", UriKind.Relative));
    }

    public void Save()
    {
        // need to install 	Syncfusion.GridExcelExport.Wpf
        AddContentView.GridControl.Model.ExportToCSV("Sample.csv");
        // if (NameTextBox.Text == String.Empty)
        // {
        //     MessageBox.Show("Vyplňte název souboru");
        // }
        //
        // string fileName = string.Concat(NameTextBox.Text, ".csv");
        // string path = Directory.GetCurrentDirectory();
        //
        // var together = string.Concat(path, "\\");
        // together = string.Concat(together, fileName);
        //
        // using (var writer = new StreamWriter(together))
        // using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        // {
        //     csv.WriteRecords(Words);
        // }
    }


    public List<TranslatedObject> Words { get; set; }



    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is MainWindow mainWindow)
        {
            TrainingView = mainWindow.TrainingView;
            AddContentView = mainWindow.AddContentControl;
        }
    }
}