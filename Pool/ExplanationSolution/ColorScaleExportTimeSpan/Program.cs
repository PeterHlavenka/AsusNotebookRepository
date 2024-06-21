// Create a worksheet.        

using System.Diagnostics;
using System.Globalization;
using Syncfusion.Licensing;
using Syncfusion.XlsIO;

SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF1cWWhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEFjX31fcHRWQWBcVE11Ww==");
var excelEngine = new ExcelEngine();
var application = excelEngine.Excel;
application.DefaultVersion = ExcelVersion.Excel2013;
var workbook = application.Workbooks.Create(1);
var worksheet = workbook.Worksheets[0];

// Load data to Apply Conditional Formatting.
worksheet["A1"].Value = "4:06:31";
worksheet["A2"].Value = "4:06:31";
worksheet["A3"].Value = "3:55:43";
worksheet["A4"].Value = "3:42:42";
worksheet["A5"].Value = "3:27:04";
worksheet["A6"].Value = "3:28:56";
worksheet["A7"].Value = "3:13:14";
worksheet["B1"].Value = "0:13:54";
worksheet["B2"].Value = "0:13:27";
worksheet["B3"].Value = "0:12:48";
worksheet["B4"].Value = "0:11:43";

// Create instance of IConditonalFormat and IConditionalFormats.
var formats = worksheet["A1:A7"].ConditionalFormats;
var format = formats.AddCondition();
format.FormatType = ExcelCFType.ColorScale;
var colorScale = format.ColorScale;
colorScale.SetConditionCount(3);

colorScale.Criteria[0].Type = ConditionValueType.Number;
colorScale.Criteria[1].Type = ConditionValueType.Number;
colorScale.Criteria[2].Type = ConditionValueType.Number;

colorScale.Criteria[0].Value = (TimeSpan.Parse("03:13:14").TotalSeconds / 84600).ToString(CultureInfo.InvariantCulture);
colorScale.Criteria[1].Value = (TimeSpan.Parse("03:28:56").TotalSeconds / 84600).ToString(CultureInfo.InvariantCulture);
colorScale.Criteria[2].Value = (TimeSpan.Parse("04:06:31").TotalSeconds / 84600).ToString(CultureInfo.InvariantCulture);




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