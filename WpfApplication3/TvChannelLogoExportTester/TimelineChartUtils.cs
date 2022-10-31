using System.Windows;
using System.Windows.Controls;
using Syncfusion.Windows.Controls.Grid;

namespace WpfApplication3
{
    /// <summary>
    /// Společné metody pro grafy, které jsou použité v průběhovce a čárovém grafu.
    /// </summary>
    internal static class TimelineChartUtils
    {
        internal static void AddHeader(this GridControl grid,
                                   
                                       FrameworkElement header,
                                 
                                       int columnNumber,
                                       int rowNumber)
        {
            StackPanel headerWrapper = new StackPanel();
            headerWrapper.Children.Add(header);
          
    

            grid.Model[rowNumber, columnNumber].CellType = GridControlCellBinding.CellType;
            grid.Model[rowNumber, columnNumber].CellItemTemplateKey = GridControlCellBinding.CellTemplate;
            grid.Model[rowNumber, columnNumber].CellValue = new GridControlCellBinding(
                    headerWrapper,
               HorizontalAlignment.Right ,
                   VerticalAlignment.Center);

            if (double.IsNaN(header.Height))
            {
                if (!double.IsInfinity(header.MaxHeight))
                    header.Height = header.MaxHeight;
                else //tak je to asi text
                    header.Height = 35 ;
            }
        }
    }
}
