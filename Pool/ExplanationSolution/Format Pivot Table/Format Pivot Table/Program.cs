using System.IO;
using Syncfusion.XlsIO;

namespace Format_Pivot_Table
{
    class Program
    {
        static void Main(string[] args)
        {
            // Nacte kontingencku ze zvoleneho excelu s kontingenckou a vyexportuje ji. Syncfusni example.
            
            
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;
                FileStream inputStream = new FileStream("../../../Data/InputTemplate.xlsx", FileMode.Open, FileAccess.Read);
                IWorkbook workbook = application.Workbooks.Open(inputStream);
                IWorksheet worksheet = workbook.Worksheets[1];
                IPivotTable pivotTable = worksheet.PivotTables[0];

                //Set BuiltInStyle
                pivotTable.BuiltInStyle = PivotBuiltInStyles.PivotStyleDark12;

                IRange range = worksheet.UsedRange.IntersectWith(worksheet.Range[1, 1, 100, 255]);
                range.AutofitColumns();
                
                
                #region Save
                //Saving the workbook
                FileStream outputStream = new FileStream("PivotTable.xlsx", FileMode.Create, FileAccess.Write);
                workbook.SaveAs(outputStream);
                #endregion

                //Dispose streams
                outputStream.Dispose();
                inputStream.Dispose();

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo = new System.Diagnostics.ProcessStartInfo("PivotTable.xlsx")
                {
                    UseShellExecute = true
                };
                process.Start();
            }
        }
    }
}
