// See https://aka.ms/new-console-template for more information

﻿using System.Diagnostics;
using Syncfusion.Drawing;
using Syncfusion.Licensing;
using Syncfusion.XlsIO;

namespace ExportToExcelR1C1;

internal class Program
{
    private static void Main(string[] args)
    {
        SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF1cWWhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEFjX31fcHRWQWBcVE11Ww==");

        using var excelEngine = new ExcelEngine();
        var application = excelEngine.Excel;
        application.DefaultVersion = ExcelVersion.Xlsx;
        var workbook = application.Workbooks.Create(1);
        var worksheet = workbook.Worksheets[0];
        
        GenerateTimeSpan(worksheet, "B", TimeSpan.FromMinutes(183));
        GenerateTimeSpan(worksheet, "D", TimeSpan.FromMinutes(20));
        
        IConditionalFormats conditionalFormats = worksheet.Range["B10:B17"].ConditionalFormats;
        IConditionalFormat conditionalFormat = conditionalFormats.AddCondition();
        conditionalFormat.FormatType = ExcelCFType.ColorScale;
        IColorScale colorScale = conditionalFormat.ColorScale;

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
        colorScale.Criteria[2].Value = "100";
         
        //Saving the workbook
        var outputStream = new FileStream("ConditionalFormat.xlsx", FileMode.Create, FileAccess.Write);
        workbook.SaveAs(outputStream);

        //Dispose streams
        outputStream.Dispose();

        var process = new Process();
        process.StartInfo = new ProcessStartInfo("ConditionalFormat.xlsx")
        {
            UseShellExecute = true
        };
        process.Start();
    }

    private static void GenerateTimeSpan(IWorksheet worksheet, string column, TimeSpan start)
    {
        for (int i = 10; i < 20; i++)
        {
            var cell = worksheet[$"{column}{i}"];
            start = start.Add(TimeSpan.FromMinutes(2));
            cell.Text = start.ToString();
        }
    }
}