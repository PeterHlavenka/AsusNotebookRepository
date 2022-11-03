using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CsvHelper.Configuration;

namespace CsvReader;

public partial class TrainingPage : UserControl
{
    public TrainingPage()
    {
        InitializeComponent();
    }
    
    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {

        string path = Directory.GetCurrentDirectory();
        string[] files = Directory.GetFiles(path, "*.csv");

           
            
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) {HasHeaderRecord = false, DetectDelimiter = true};
        using var reader = new StreamReader(files[0]);
        using var csv = new CsvHelper.CsvReader(reader, config);
            
        Words = csv.GetRecords<Animal>().ToList();
        CurrentPositon = 0;
        TextBlock.Text = Words.ElementAt(CurrentPositon).Cz;
        CounterTextBlock.Text = Words.ElementAt(CurrentPositon).Position.ToString();
    }

    private List<Animal> Words { get; set; }
       
    private int CurrentPositon { get; set; }

    private void Count_OnClick(object sender, RoutedEventArgs e)
    {
        CurrentPositon += 1;
        var newWord = Words.FirstOrDefault(d => int.Parse(d.Position) == CurrentPositon);

        if (newWord == null)
        {
            CurrentPositon = 1;
        }
            
        newWord = Words.First(d => int.Parse(d.Position) == CurrentPositon);
        TextBlock.Text = newWord.Cz;
        CounterTextBlock.Text = newWord.Position.ToString();
    }
}