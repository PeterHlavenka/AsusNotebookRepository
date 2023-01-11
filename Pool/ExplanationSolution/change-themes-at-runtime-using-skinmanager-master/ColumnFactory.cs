using System;
using System.Windows;
using System.Windows.Data;
using DataGrid_Themes.CustomRenderers;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;

namespace DataGrid_Themes
{
    public class ColumnFactory
    {
        public const string IsSelectedColumnName = nameof(Employee.Selected);  // CellWrapper.IsSelected
        private readonly Func<string, object> m_findResource;

        public ColumnFactory(Func<string, object> findResource)
        {
            m_findResource = findResource;
        }

        internal GridTemplateColumn CreateSelectionColumn()
        {
        
            return new GridTemplateColumn
            {
                HeaderText = "Selection",
                CellTemplate = (DataTemplate) m_findResource("SelectionTemplate"),
                FilterRowEditorType = SelectionFilterRowRenderer.Id,
                ColumnFilter = ColumnFilter.Value,
                FilterRowOptionsVisibility = Visibility.Collapsed,
                AllowFiltering = false,
                ValueBinding = new Binding(IsSelectedColumnName),
                MappingName = IsSelectedColumnName,
                Width = 56,
                AllowResizing = false
            };
        }
    }
}