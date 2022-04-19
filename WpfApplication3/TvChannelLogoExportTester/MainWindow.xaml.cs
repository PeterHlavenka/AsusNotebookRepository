using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Syncfusion.Pdf;
using Syncfusion.Windows.Controls.Grid;


namespace WpfApplication3
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly object m_tesst;
        
        public MainWindow()
        {
            InitializeComponent();

            //ScrollViewer defined here
            var scrollViewer = new ScrollViewer();
            //GridControl defined here
            var grid = new GridControl();
            //GridControl set as the content of the ScrollViewer
            scrollViewer.Content = grid;
            //To bring the Grid control to the view, ScrollViewer should be set as a child of LayoutRoot      
            LayoutRoot.Children.Add(scrollViewer);

            //Specifying row and column count
            grid.Model.RowCount = 6;
            grid.Model.ColumnCount = 6;

            grid.QueryCellInfo += GridControlQueryCellInfo;

            //Assigning values by handling the QueryCellInfo event
            void GridControlQueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
            {
                if (e.Cell.RowIndex != 1 || e.Cell.ColumnIndex != 1)
                {
                    e.Style.CellValue = $"{e.Cell.RowIndex}/{e.Cell.ColumnIndex}";
                }
            }
            
            // view specific cell color
         //   grid.PrepareRenderCell += GridPrepareRenderCell;
            void GridPrepareRenderCell(object sender, GridPrepareRenderCellEventArgs e)
            {
                if (e.Cell.RowIndex > 0 && e.Cell.ColumnIndex > 0)
                {
                    if (e.Cell.RowIndex % 2 == 0)
                        e.Style.Background = Brushes.LightSkyBlue;
                }
            }

            
            
            
            
            //Defines the base styles named PinkStyle and GreenStyle.
            GridBaseStyle baseStyle1 = new GridBaseStyle();
            baseStyle1.Name = "PinkStyle";
            baseStyle1.StyleInfo.Background = Brushes.LightPink;
            baseStyle1.StyleInfo.Foreground = Brushes.Maroon;

            GridBaseStyle baseStyle2 = new GridBaseStyle();
            baseStyle2.Name = "GreenStyle";
            baseStyle2.StyleInfo.Background = Brushes.PaleGreen;
            baseStyle2.StyleInfo.Foreground = Brushes.Olive;

//Add the above styles to the grid base styles collection.
            grid.Model.BaseStylesMap.Add(baseStyle1);
            grid.Model.BaseStylesMap.Add(baseStyle2);

//Applying base styles.
            // for (int i = 1; i <= grid.Model.RowCount; i++)
            // {
            //
            //     for (int j = 1; j <= grid.Model.ColumnCount; j++)
            //     {
            //
            //         if(j ==2)
            //             grid.Model[i, j].BaseStyle = "PinkStyle";
            //
            //         else
            //             grid.Model[i,j].BaseStyle = "GreenStyle";
            //     }
            // }

            var rowNumber = 1;
            var columnNumber = 1;

            var rectangle = new StackPanel();
            rectangle.Height = 100;
            rectangle.Width = 100;
            rectangle.Background = Brushes.Red;

             // grid.Model[rowNumber, columnNumber].CellType = GridControlCellBinding.CellType;
             // grid.Model[rowNumber, columnNumber].CellItemTemplateKey = GridControlCellBinding.CellTemplate;
             //
             grid.Model.RowHeights[rowNumber] = 120;
             grid.Model.ColumnWidths[columnNumber] = 120;
             //
             m_tesst = LoadXamlFromFile("Doma_tv.xaml");
             //
             // grid.Model[rowNumber, columnNumber].CellValue = tesst;
             
             grid.AddHeader((FrameworkElement)m_tesst, 1,1);
             
             grid.InvalidateVisual();
        }
        
        public static object LoadXamlFromFile(string fileName) 
        {
            using (Stream s = File.OpenRead(fileName)) 
                return XamlReader.Load(s, new ParserContext { 
                    BaseUri = new Uri(Path.GetFullPath(fileName), UriKind.Absolute)});
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            // var test = new ScheduleExporter();
            // var document = new PdfDocument(PdfConformanceLevel.Pdf_A1B);
            // test.FillPdfDocument((FrameworkElement) m_tesst, document, new PdfExportSettings(), new InfoContent());
            //
            // document.Save("Output.pdf");
            // document.Close();
        }


    }
}