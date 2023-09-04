using System;
using System.Drawing;
using System.IO;
using System.Windows;
using Syncfusion.ExcelToPdfConverter;
using Syncfusion.Licensing;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;

namespace MergeTwoExcelFilesAndConvertItToPdf;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        SyncfusionLicenseProvider.RegisterLicense("MjY2MDY4NkAzMjMyMmUzMDJlMzBUV0NndUlYRFRhUWdJck5zS1FFR2dKUEJvMC9BVUdGbXNkM0RsT3k2OFo0PQ==");
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        // Load the Excel files
        using (var excelEngine = new ExcelEngine())
        {
            var workbook1 = @"D:\AsusNotebookRepository\Pool\ExplanationSolution\MergeTwoExcelFilesAndConvertItToPdf\Časová analýza.xlsx";
            var workbook2 = @"D:\AsusNotebookRepository\Pool\ExplanationSolution\MergeTwoExcelFilesAndConvertItToPdf\Časová analýza1.xlsx";
            var mergedFilePath = @"D:\AsusNotebookRepository\Pool\ExplanationSolution\MergeTwoExcelFilesAndConvertItToPdf\Merged.xlsx";

            MergeExcelFiles(workbook1, workbook2, mergedFilePath);
        }
    }

    private void MergeExcelFiles(string file1Path, string file2Path, string mergedFilePath)
    {
        using (var excelEngine = new ExcelEngine())
        {
            var application = excelEngine.Excel;

            // Open the source workbooks
            var workbook1 = application.Workbooks.Open(file1Path);
            var workbook2 = application.Workbooks.Open(file2Path);
            var worksheet1 = workbook1.Worksheets[0];
            var worksheet2 = workbook2.Worksheets[0];
            var mergedWorkbook = application.Workbooks.Create();
            var mergedWorksheet = mergedWorkbook.Worksheets[0];
            for (var row = 1; row <= worksheet1.UsedRange.LastRow; row++)
            for (var col = 1; col <= worksheet1.UsedRange.LastColumn; col++)
                mergedWorksheet[row, col].Value = worksheet1[row, col].Value;
            var startRow = worksheet1.UsedRange.LastRow + 1;
            for (var row = 1; row <= worksheet2.UsedRange.LastRow; row++)
            for (var col = 1; col <= worksheet2.UsedRange.LastColumn; col++)
                mergedWorksheet[startRow + row - 1, col].Value = worksheet2[row, col].Value;
            mergedWorkbook.SaveAs(mergedFilePath);
        }
    }
}