using System.IO;
using Syncfusion.XlsIO;

namespace XlsxReaderNetCoreProject;

public class Helper
{
    public void Save(IWorkbook workbook)
    {
        using var excelEngine = new ExcelEngine();
        //Set the version of the workbook
        workbook.Version = ExcelVersion.Excel2013;

        //Save the workbook as stream
        using var outputStream = new MemoryStream();
        workbook.SaveAs(outputStream);
    }

    public IWorkbook OpenFile(ExcelEngine excelEngine, string inputFileName)
    {
        //Initialize application
        var app = excelEngine.Excel;

        //Set default application version as Xlsx
        app.DefaultVersion = ExcelVersion.Xlsx;

        //Load the file into stream
        using var inputStream = new FileStream(inputFileName, FileMode.Open);

        //Open existing Excel workbook from the specified location
        var workbook = app.Workbooks.Open(inputStream, ExcelOpenType.Automatic);

        return workbook;
    }
}