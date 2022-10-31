using System.Windows.Controls;
using System.Windows.Media;
using Syncfusion.Licensing;
using Syncfusion.Windows.Controls.Grid;

namespace SyncfusionGridControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        { 
            SyncfusionLicenseProvider.RegisterLicense("NjEyNzcyQDMyMzAyZTMxMmUzMEs4b1N6clUwKy8wbkltVjVuNkFwZHlBSURBeFFSUnlTdXhFNGtDMVZQdEU9");
            InitializeComponent();

            GridControl.Model.RowCount = 100;
            GridControl.Model.ColumnCount = 3;
            
            // 1.You can populate data by looping through the cells in the Grid control. The following code explains this scenario.
            // for (int i = 0; i < 100; i++)
            // {
            //     for (int j = 0; j < 20; j++)
            //     {
            //         GridControl.Model[i, j].CellValue = string.Format("{0}/{1}", i, j);
            //     }
            // }
            
            
            
            // 2.You can populate data by handling the QueryCellInfo event of gridControl. This will load the data in and on-demand basis, ensuring optimized performance.
            GridControl.QueryCellInfo += GridControlQueryCellInfo;
            GridControl.PrepareRenderCell += new GridPrepareRenderCellEventHandler(GridPrepareRenderCell);  // obarvim bunku
            
            
            
            
            void GridControlQueryCellInfo(object sender, Syncfusion.Windows.Controls.Grid.GridQueryCellInfoEventArgs e)
            {
                e.Style.CellValue= $"{e.Cell.RowIndex}/{e.Cell.ColumnIndex}";
            }   
        }

        private void GridPrepareRenderCell(object sender, GridPrepareRenderCellEventArgs e)
        {
            if (e.Cell.RowIndex == 0 && e.Cell.ColumnIndex == 0)
            {
                e.Style.Background = Brushes.GreenYellow;
            }
        }
    }
}