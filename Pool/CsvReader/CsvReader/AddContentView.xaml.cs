using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Syncfusion.UI.Xaml.Grid.Converter;

namespace CsvReader;

public partial class AddContentView : UserControl
{
    private List<TranslatedObject> m_list = new();
    public AddContentView()
    {
        InitializeComponent();

        for (int i = 1; i < 101; i++)
        {
            m_list.Add(new TranslatedObject(i.ToString(), string.Empty, String.Empty, String.Empty));
        }

        SfDataGrid.ItemsSource = m_list;
    }

    public void Save()
    {
        var options = new ExcelExportingOptions();
        var excelEngine = SfDataGrid.ExportToExcel(SfDataGrid.View, options);
        var workBook = excelEngine.Excel.Workbooks[0];
        workBook.SaveAs("Sample.xlsx");
        // workBook.SaveAs("Sample.csv", ",");
    }
}