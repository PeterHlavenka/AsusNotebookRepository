using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Syncfusion.Windows.Controls.Grid;
using System.Reflection;
using Syncfusion.SfSkinManager;
using SyncfusionThemeRegistrator;
using System.Windows.Forms;
using Microsoft.Win32;

namespace GridControlSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int m_scrollJump;

        public MainWindow()
        {
            InitializeComponent();

            SfThemeRegistrator.RegisterTheme(VisualStyles.Office2019Colorful);
            SfThemeRegistrator.SetVisualStyle(this);
            SfThemeRegistrator.SetTheme(this);
            
            grid.Model.RowCount = 930;
            grid.Model.ColumnCount = 10;
            grid.Model.HeaderRows = 2;
            grid.Model.HeaderColumns = 3;
            grid.Model.HeaderStyle.Background = new SolidColorBrush(Colors.WhiteSmoke);
            grid.Model.Options.ActivateCurrentCellBehavior = GridCellActivateAction.DblClickOnCell;

            grid.Model.QueryCellInfo += Model_QueryCellInfo;
            grid.Model.SelectionChanged += Model_SelectionChanged;
            
            Type t = typeof(System.Windows.Forms.SystemInformation);            
            PropertyInfo[] pi = t.GetProperties().Where(d => d.Name.StartsWith("Mouse")).ToArray(); 
            
            for( int i=0; i<pi.Length; i++ )
            {
                var test = pi[i].GetIndexParameters();
                object propval = pi[i].GetValue(SystemInformation.PowerStatus, null);
                var proper = pi[i].GetValue(null, null);
            }

            
            // Z registru vytahnu klic podle jmena a vezmu si jeho hodnotu pro scrollovani:
            var rk = Registry.CurrentUser;
            var obj = rk.OpenSubKey("Control Panel")?.OpenSubKey("Desktop")?.GetValue("WheelScrollLines");
            
            if (int.TryParse(obj?.ToString(), out var result) && result > 0)
                m_scrollJump = result;
        }

        private void Model_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            //Invalidate the corresponding Header rows and colums when selection is changed.
            grid.Model.InvalidateCell(GridRangeInfo.Cols(0, grid.Model.HeaderColumns - 1));
            grid.Model.InvalidateCell(GridRangeInfo.Rows(0, grid.Model.HeaderRows - 1));
            grid.Model.InvalidateVisual();
        }

        private void Model_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            //Modify the background of header cells based on cell selection.
            var test = e.Cell.RowIndex == grid.Model.HeaderRows - 1;
            var bla = grid.Model.SelectedRanges;
            
            var columnInfo = GridRangeInfo.Col(e.Cell.ColumnIndex);  // creates new GridRangeInfo for the specified column
            var dva = grid.Model.SelectedRanges.AnyRangeIntersects(GridRangeInfo.Col(e.Cell.ColumnIndex));
            
            var rowInfo = GridRangeInfo.Row(e.Cell.RowIndex); // creates new GridRangeInfo for the specified row
            var tri = grid.Model.SelectedRanges.AnyRangeIntersects(GridRangeInfo.Row(e.Cell.RowIndex));
            
            if (grid.Model.SelectedRanges.AnyRangeIntersects(GridRangeInfo.Col(e.Cell.ColumnIndex))
                || grid.Model.SelectedRanges.AnyRangeIntersects(GridRangeInfo.Row(e.Cell.RowIndex)))
            {
                if (e.Style.CellType == "Static")  //  Static == je to header
                {
                    e.Style.Background = new SolidColorBrush(Colors.Chartreuse);
                    e.Style.Font.FontWeight = FontWeights.Bold;
                }
                
            }

            // Cisla radku + pismena pro sloupce
            if (e.Cell.RowIndex == 0 && e.Cell.ColumnIndex == 0)
                return;
            if (e.Cell.RowIndex == 0)
                e.Style.CellValue = GridRangeInfo.GetAlphaLabel(e.Cell.ColumnIndex);
            else if(e.Cell.ColumnIndex == 0)
                e.Style.CellValue = e.Cell.RowIndex;
        }

        // Scrolluju na zaklade uzivatelovo hodnoty kterou ma v registrech a nastavuje se ve windows settings v sekci Mouse/Choose how many lines to scroll each time
        private void Grid_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (e.Delta > 0)
            {
                for (int i = 0; i < m_scrollJump; i++)
                {
                    grid.LineUp();
                    grid.InvalidateArrange();
                }
            }

            if (e.Delta < 0)
            {
                for (int i = 0; i < m_scrollJump; i++)
                {
                    grid.LineDown();
                    grid.InvalidateArrange();
                }
            }
        }
    }
}
