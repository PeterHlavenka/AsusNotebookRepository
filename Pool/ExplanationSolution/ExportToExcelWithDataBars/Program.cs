using Syncfusion.Drawing;
using Syncfusion.Licensing;
using Syncfusion.XlsIO;

SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF1cWWhIf0x0Qnxbf1xzZFFMZFlbRHJPMyBoS35RdURiW31edHBQRmReVk1+");

using var excelEngine = new ExcelEngine();
var application = excelEngine.Excel;
application.DefaultVersion = ExcelVersion.Excel2013;

var fileStream = new FileStream("SampleTemplate.xlsx", FileMode.Open, FileAccess.Read);
var workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
var worksheet = workbook.Worksheets[0];

//Create data bars for the data in specified range
var conditionalFormats = worksheet.Range["C7:C46"].ConditionalFormats;
var conditionalFormat = conditionalFormats.AddCondition();
conditionalFormat.FormatType = ExcelCFType.DataBar;
var dataBar = conditionalFormat.DataBar;

//Set the constraints
dataBar.MinPoint.Type = ConditionValueType.LowestValue;
dataBar.MaxPoint.Type = ConditionValueType.HighestValue;

//Set color for Bar
dataBar.BarColor = Color.FromArgb(156, 208, 243);

//Hide the values in data bar
dataBar.ShowValue = false;
dataBar.BarColor = Color.Aqua;

//Create color scales for the data in specified range
conditionalFormats = worksheet.Range["D7:D46"].ConditionalFormats;
conditionalFormat = conditionalFormats.AddCondition();
conditionalFormat.FormatType = ExcelCFType.ColorScale;
var colorScale = conditionalFormat.ColorScale;

//Sets 3 - color scale
colorScale.SetConditionCount(3);
colorScale.Criteria[0].FormatColorRGB = Color.FromArgb(230, 197, 218);
colorScale.Criteria[0].Type = ConditionValueType.LowestValue;
colorScale.Criteria[0].Value = "0";

colorScale.Criteria[1].FormatColorRGB = Color.FromArgb(244, 210, 178);
colorScale.Criteria[1].Type = ConditionValueType.Percentile;
colorScale.Criteria[1].Value = "50";

colorScale.Criteria[2].FormatColorRGB = Color.FromArgb(245, 247, 171);
colorScale.Criteria[2].Type = ConditionValueType.HighestValue;
colorScale.Criteria[2].Value = "0";

conditionalFormat.FirstFormulaR1C1 = "=R[1]C[0]";
conditionalFormat.SecondFormulaR1C1 = "=R[1]C[1]";

//Create icon sets for the data in specified range
conditionalFormats = worksheet.Range["E7:E46"].ConditionalFormats;
conditionalFormat = conditionalFormats.AddCondition();
conditionalFormat.FormatType = ExcelCFType.IconSet;
var iconSet = conditionalFormat.IconSet;

//Apply three symbols icon and hide the data in the specified range
iconSet.IconSet = ExcelIconSetType.ThreeSymbols;
iconSet.IconCriteria[1].Type = ConditionValueType.Percent;
iconSet.IconCriteria[1].Value = "50";
iconSet.IconCriteria[2].Type = ConditionValueType.Percent;
iconSet.IconCriteria[2].Value = "50";
iconSet.ShowIconOnly = true;

//Saving the workbook as stream
var stream = new FileStream("ConditionalFormatting.xlsx", FileMode.Create, FileAccess.ReadWrite);
workbook.SaveAs(stream);
stream.Dispose();


// Otevreni excelu
System.Diagnostics.Process process = new System.Diagnostics.Process();
process.StartInfo = new System.Diagnostics.ProcessStartInfo("ConditionalFormatting.xlsx")
{
    UseShellExecute = true
};
process.Start();