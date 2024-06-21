// Create a worksheet.        

using System.Diagnostics;
using Syncfusion.Licensing;
using Syncfusion.XlsIO;

SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF1cWWhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEFjX31fcHRWQWBcVE11Ww==");
var excelEngine = new ExcelEngine();
var application = excelEngine.Excel;
application.DefaultVersion = ExcelVersion.Excel2013;
var workbook = application.Workbooks.Create(1);
var worksheet = workbook.Worksheets[0];

// Load data to Apply Conditional Formatting.
worksheet["A1"].Text = "ColorScale";
worksheet["A2"].Number = 10;
worksheet["A3"].Number = 20;
worksheet["A4"].Number = 30;
worksheet["A5"].Number = 40;
worksheet["A6"].Number = 50;
worksheet["A7"].Number = 60;
worksheet["A8"].Number = 70;
worksheet["A9"].Number = 80;
worksheet["A10"].Number = 90;
worksheet["A11"].Number = 100;

// Create instance of IConditonalFormat and IConditionalFormats.
var formats = worksheet["A2:A11"].ConditionalFormats;
var format = formats.AddCondition();

// Set FormatType as ColorScale.
format.FormatType = ExcelCFType.ColorScale;
var colorScale = format.ColorScale;

// Set 3 as count for color scale
colorScale.SetConditionCount(3);

// Change Threshold value for 2nd object in Critera list.
colorScale.Criteria[1].Value = "20";

// Save and Dispose.
var outputStream = new FileStream("ConditionalFormat.xlsx", FileMode.Create, FileAccess.Write);
workbook.SaveAs(outputStream);
workbook.Close();
excelEngine.Dispose();

var process = new Process();
process.StartInfo = new ProcessStartInfo("ConditionalFormat.xlsx")
{
    UseShellExecute = true
};
process.Start();