using Syncfusion.Licensing;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;

SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF1cWWhIf0x0Qnxbf1xzZFFMZFlbRHJPMyBoS35RdURiW31edHBQRmReVk1+");

using (var excelEngine = new ExcelEngine())
{
    var application = excelEngine.Excel;
    application.DefaultVersion = ExcelVersion.Excel2013;
    
    using (var inputStream = new FileStream(@"..\..\..\ConditionalFormatting.xlsx", FileMode.OpenOrCreate))
    {
        var workbook = application.Workbooks.Open(inputStream);
        using (var fs = new FileStream("Output.html", FileMode.Create))
        {
            workbook.SaveAsHtml(fs, HtmlSaveOptions.Default);
        }
    }
}

Console.WriteLine("Conversion completed.");