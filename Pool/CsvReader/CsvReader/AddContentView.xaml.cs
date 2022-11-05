using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Syncfusion.Windows.ComponentModel;
using Syncfusion.Windows.Controls.Grid;

namespace CsvReader;

public partial class AddContentView : UserControl
{
    private List<TranslatedObject> m_list = new();
    public AddContentView()
    {
        InitializeComponent();
        GridControl.Model.TableStyle.ReadOnly = false;
        GridControl.Model.RowCount = 35;
        GridControl.Model.ColumnCount = 4;

        for (int i = 1; i < 101; i++)
        {
            m_list.Add(new TranslatedObject(i.ToString(), string.Empty, String.Empty, String.Empty));
        }

        m_list.First().Cz = "ceskyNazev";
        
        GridControl.Model.QueryCellInfo += (s, e) =>
        {
            if (e.Style.RowIndex > 0 && e.Style.ColumnIndex > 0)
            {
                var obj = m_list.ElementAt(e.Style.RowIndex - 1);
                if (e.Cell.ColumnIndex == 1)
                {
                    e.Style.CellValue = obj.Cz;
                }
                if (e.Cell.ColumnIndex == 2)
                {
                    e.Style.CellValue = obj.En;
                }
                if (e.Cell.ColumnIndex == 3)
                {
                    e.Style.CellValue = obj.De;
                }
               // e.Style.CellValue = string.Format("R{0}C{1}", e.Cell.RowIndex, e.Cell.ColumnIndex);
            }
                
        };
        //this.GridControl.Model.ColumnWidths[0] = 80;

        GridControl.Model.Options.ColumnSizer = GridControlLengthUnitType.AutoWithLastColumnFill;
    }

    private void GridControl_OnCurrentCellEditingComplete(object sender, SyncfusionRoutedEventArgs args)
    {
        if (sender is GridControl gridControl)
        {
            var neco = gridControl.CurrentCell.CellRowColumnIndex;
            var obj = m_list.ElementAt(neco.RowIndex - 1);

            if (neco.ColumnIndex == 1)
            {
                var str = obj.Cz;
            }
        }
       
    }

    private void GridControl_OnSelectionChanging(object sender, GridSelectionChangingEventArgs e)
    {
        var test = sender;
    }
}