﻿#region

using System.Windows;
using Adwind.Export;
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
                var destinationWorkbook = application.Workbooks.Open("11h dayparts.xlsx");

                var workbookToAdd = application.Workbooks.Open("11h kids.xlsx");
                destinationWorkbook.Worksheets.AddCopy(workbookToAdd.Worksheets);
                
                workbookToAdd = application.Workbooks.Open("11h telegrid.xlsx");
                destinationWorkbook.Worksheets.AddCopy(workbookToAdd.Worksheets);
                
                workbookToAdd = application.Workbooks.Open("11h ttv.xlsx");
                destinationWorkbook.Worksheets.AddCopy(workbookToAdd.Worksheets);

                
                workbookToAdd.Close();

                MergeAnalysesInWorksheet.Merge(destinationWorkbook, "nazev listu");

                //Save the Excel document
                destinationWorkbook.SaveAs("Output.xlsx");
                destinationWorkbook.Close();
            }
        }
    }
}