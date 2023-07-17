// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using Syncfusion.Licensing;
using Syncfusion.XlsIO;
using XlsxReaderNetCoreProject;

SyncfusionLicenseProvider.RegisterLicense("MjQ5NDI1N0AzMjMyMmUzMDJlMzBPdnZkdEw4L1haUDk3MlJ6UUxFSXUvcEROUVFVeWI1WWplRGlycEg5QjhBPQ==");

var helper = new Helper();
string directory = Directory.GetCurrentDirectory();
directory = directory[..^17];
string name = "Sample.xlsx";
string path = Path.Combine(directory, name);

using var excelEngine = new ExcelEngine();
var workbook = helper.OpenFile(excelEngine, path);

//Access the first worksheet
var worksheet = workbook.Worksheets[0];
        
//Access the used range of the Excel file
var usedRange = worksheet.UsedRange;
var lastRow = usedRange.LastRow;
var lastColumn = usedRange.LastColumn;
            
//Iterate the cells in the used range and print the cell values
for (var row = 1; row <= lastRow; row++)
{
    for (var col = 1; col <= lastColumn; col++)
    {
        Console.WriteLine(worksheet[row, col].Value);
    }
}

//Save workbook
string outputFileName = Path.Combine(directory, "output.xlsx");
using var fileStream = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
workbook.SaveAs(fileStream);

outputFileName = Path.Combine(directory, "output.html");
using var htmlFileStream = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
workbook.SaveAsHtml(htmlFileStream);