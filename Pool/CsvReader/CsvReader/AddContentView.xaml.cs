using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Win32;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;

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
        var saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
        string path;
        if (saveFileDialog.ShowDialog() == true)
        {
            path = saveFileDialog.FileName;
            var options = new ExcelExportingOptions();
            var excelEngine = SfDataGrid.ExportToExcel(SfDataGrid.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            workBook.SaveAs(path);
        }
    }

    public void OpenFile(FileInfo inputFileName)
    {
        using (var excelEngine = new ExcelEngine())
        {
            //Initialize application
            var app = excelEngine.Excel;

            //Set default application version as Xlsx
            app.DefaultVersion = ExcelVersion.Xlsx;

            //Open existing Excel workbook from the specified location
            var workbook = app.Workbooks.Open(inputFileName.FullName, ExcelOpenType.Automatic);

            //Access the first worksheet
            var worksheet = workbook.Worksheets[0];

            //Access the used range of the Excel file
            var usedRange = worksheet.UsedRange;
            var lastRow = usedRange.LastRow;
            var lastColumn = usedRange.LastColumn;
            //Iterate the cells in the used range and print the cell values
            var translatedObjects = new List<TranslatedObject>();
            var obj = new TranslatedObject();
            for (var row = 2; row <= lastRow; row++) // indexy od 1 + preskakuju header row
            {
                for (var col = 1; col <= lastColumn; col++)
                {
                    var propertyInfo = obj.GetType().GetProperties().ElementAt(col - 1);
                    propertyInfo.SetValue(obj, worksheet[row, col].Value);
                }

                translatedObjects.Add(new TranslatedObject(obj.Position, obj.Cz, obj.En, obj.De));
            }

            SfDataGrid.ItemsSource = translatedObjects;
        }
    }
}