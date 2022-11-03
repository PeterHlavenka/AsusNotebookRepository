using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using CsvHelper;

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
        
        Words = new List<Animal>();

        for (int i = 1; i < 100; i++)
        {
            Words.Add(new Animal(i.ToString(), string.Empty, String.Empty, String.Empty));
        }
        
        DataGrid.ItemsSource = Words;
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
        if (AddControlVisibility == Visibility.Visible)
        {
            Save();
        }
        
        AddControlVisibility = AddControlVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        ButtonImageSource = AddControlVisibility == Visibility.Visible? 
            new BitmapImage(new Uri(".\\Icons\\Ok.png", UriKind.Relative)):
            new BitmapImage(new Uri(".\\Icons\\Plus.png", UriKind.Relative));
    }

    private void Save()
    {
        if (NameTextBox.Text == String.Empty)
        {
            MessageBox.Show("Vyplňte název souboru");
        }
        
        string fileName = string.Concat(NameTextBox.Text, ".csv");
        string path = Directory.GetCurrentDirectory();

        var together = string.Concat(path, "\\");
        together = string.Concat(together, fileName);
        
        using (var writer = new StreamWriter(together))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(Words);
        }
    }


    public List<Animal> Words { get; set; }



    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    
}