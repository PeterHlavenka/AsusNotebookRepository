using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CsvReader;

public partial class AddContentUserControl : UserControl
{
    public AddContentUserControl()
    {
        InitializeComponent();

        Words = new List<Animal>();

        for (int i = 1; i < 100; i++)
        {
            Words.Add(new Animal(i, string.Empty, String.Empty, String.Empty));
        }
        
        DataGrid.ItemsSource = Words;
    }
    
    public List<Animal> Words { get; set; }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var test = Words;
    }
}