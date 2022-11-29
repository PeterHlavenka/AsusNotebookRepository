using Syncfusion.XlsIO;

namespace ExcelToPdfExporter
{
    public static class SheetExport
    {
        public static void AutoResizeColsRows(IWorksheet sheet) //, ExportSettings exportSettings, PivotTableView tableView = null)
        {
            // if (!exportSettings.ExportSettingsBase.WithFormat)
            //     return;
            // // AutoFit column and rows widths
            // if (exportSettings.ExcelExportSettings.AutosizeColumns)
            // {
                //s_log.Debug("AutoFitColumns");

                // prvních max. 100 řádků
                IRange range = sheet.UsedRange.IntersectWith(sheet.Range[1, 1, 100, 255]);
                if (range != null)
                    range.AutofitColumns();

            //     // max. velikost 
            //     if (exportSettings.ExcelExportSettings.MaxColumnWidth != 0)
            //         foreach (IRange column in sheet.UsedRange.Columns)
            //         {
            //             if (column.ColumnWidth > exportSettings.ExcelExportSettings.MaxColumnWidth)
            //                 column.ColumnWidth = exportSettings.ExcelExportSettings.MaxColumnWidth;
            //         }
            // }
            //
            // for (int i = 0; i < (tableView?.ColumnCount ?? 0); i++)
            // {
            //     var columnWidth = tableView.ColumnWidths.GetWidth(i);
            //     if (columnWidth > 0)
            //         sheet.Columns[i].ColumnWidth = sheet.PixelsToColumnWidth((int) columnWidth);
            // }
            //
            // if (exportSettings.ExcelExportSettings.AutosizeRows)
            // {
            //    // s_log.Debug("AutoFitRows");
            //     sheet.UsedRange.EntireRow.AutofitRows();
            //
            //     if (exportSettings.ExcelExportSettings.UseMaxRowHeight)
            //         foreach (IRange row in sheet.UsedRange.Rows)
            //         {
            //             if (row.RowHeight > exportSettings.ExcelExportSettings.MaxRowHeight)
            //                 row.RowHeight = exportSettings.ExcelExportSettings.MaxRowHeight;
            //         }
            // }
           // s_log.Debug("ApplySettings done.");
        }
    }
}