#region

using System.Windows;
using Syncfusion.XlsIO;

#endregion

namespace ExcelMergeSample
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            using (var excelEngine = new ExcelEngine())
            {
                //Instantiate the Excel application object
                var application = excelEngine.Excel;

                //Set the default application version
                application.DefaultVersion = ExcelVersion.Excel2016;

                //Load the existing Excel workbook into IWorkbook
                var destinationWorkbook = application.Workbooks.Open("Rovning_months.xlsx");

                var workbookToAdd = application.Workbooks.Open("Rovning_weeks.xlsx");

                destinationWorkbook.Worksheets.AddCopy(workbookToAdd.Worksheets);

                //Save the Excel document
                destinationWorkbook.SaveAs("Output.xlsx");
            }
        }
    }
}