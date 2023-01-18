using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using DataGrid_Themes.Controls;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.RowFilter;

namespace DataGrid_Themes.CustomRenderers
{
    public class SelectionFilterRowRenderer : GridFilterRowCellRenderer<SelectionFilterView, SelectionFilterView>
    {
        public const string Id = "SelectionFilterRowRenderer";
        private SelectionFilterView m_displayElement;


        public override void OnInitializeDisplayElement(DataColumnBase dataColumn, SelectionFilterView uiElement, object dataContext)
        {
            m_displayElement = uiElement;
            SetFilterView(dataColumn, uiElement);
            m_displayElement.TextFilter =
                dataColumn.GridColumn.FilterPredicates
                    .Select(fp => (bool?) fp.FilterValue)
                    .Select(Convert)
                    .Join(string.Empty);
            uiElement.IsHitTestVisible = false;
        }

        protected override SelectionFilterView OnCreateDisplayUIElement()
        {
            return m_displayElement ?? base.OnCreateDisplayUIElement();
        }

        public override void OnInitializeEditElement(DataColumnBase dataColumn, SelectionFilterView uiElement, object dataContext)
        {
            SetFilterView(dataColumn, uiElement);
            uiElement.TextFilterChanged -= OnFilterChanged;
            uiElement.TextFilterChanged += OnFilterChanged;
        }

        protected override void OnUnwireEditUIElement(SelectionFilterView uiElement)
        {
            uiElement.TextFilterChanged -= OnFilterChanged;
            base.OnUnwireEditUIElement(uiElement);
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            if (!HasCurrentCellState)
                return;

            IsValueChanged = true;
            var view = (SelectionFilterView)sender;
            List<bool?> selected = new List<bool?>();
            if (view.IsFalseSelected) selected.Add(false);
            if (view.IsOtherSelected) selected.Add(null);
            if (view.IsTrueSelected) selected.Add(true);
            var predicates = CreateFilterPredicates(selected);
            string filterText = selected.Select(Convert).Join(string.Empty);
            ApplyFilters(predicates, filterText);
            m_displayElement.TextFilter = filterText;
            view.TextFilter = filterText;
            IsValueChanged = false;
        }

        private string Convert(bool? item)
        {
            switch (item)
            {
                case true:
                    return SelectionFilterView.True;
                case null:
                    return SelectionFilterView.Null;
                default:
                    return SelectionFilterView.False;
            }
        }

        private List<FilterPredicate> CreateFilterPredicates(List<bool?> selectedItems)
        {
            return selectedItems.ToListOf(GetFilterPredicate);
        }

        private FilterPredicate GetFilterPredicate(bool? value)
        {
            return new FilterPredicate
            {
                FilterBehavior = FilterBehavior.StronglyTyped,
                FilterType = FilterType.Equals,
                FilterValue = value,
                IsCaseSensitive = false,
                PredicateType = PredicateType.OrElse
            };
        }

        private void SetFilterView(DataColumnBase dataColumn, SelectionFilterView uiElement)
        {
            GridColumn gridColumn = dataColumn.GridColumn;
            uiElement.FocusVisualStyle = null;
            Binding binding = new Binding
            {
                Path = new PropertyPath("FilterRowText"),
                Mode = BindingMode.TwoWay,
                Source = gridColumn,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            uiElement.SetBinding(SelectionFilterView.TextFilterProperty, binding);
            uiElement.TextFilter = FilterRowCell?.DataColumn?.GridColumn?.FilterRowText as string ?? string.Empty;
        }
        

    }

    public static class Extensions
    {
        public static string Join( this IEnumerable<object> items, string separator)
        {
            if (items == null)
                return "";

            return string.Join(separator, items);
        }
        
        public static List<TResult> ToListOf<T, TResult>( this IEnumerable<T> items, 
             Func<T, TResult> selector)
        {
            return new List<TResult>(items.Select(selector));
        }
    }
}