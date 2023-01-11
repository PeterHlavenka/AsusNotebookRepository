#region Copyright Syncfusion Inc. 2001 - 2020
// Copyright Syncfusion Inc. 2001 - 2020. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.Windows.Shared;
using System;
using System.Linq;
using System.Windows;
using Syncfusion.Data.Extensions;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;

namespace DataGrid_Themes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ChromelessWindow
    {
        public MainWindow()
        {
           
            SfSkinManager.ApplyStylesOnApplication = true;

            InitializeComponent();


            var columnFactory = new ColumnFactory(FindResource);
            var selectionColumn = columnFactory.CreateSelectionColumn();  // vytvorim sloupec pro prikazovani a zakazovani bloku
            
            // pridam ho na zacatek gridu 
            var test = AdwDataGrid.Columns.Select(d => d).ToList<GridColumn>();
            AdwDataGrid.Columns.Clear();
            AdwDataGrid.Columns.Add(selectionColumn); 
            foreach (var columns in test)
            {
                AdwDataGrid.Columns.Add(columns);
            }

            if (AdwDataGrid.DataContext is ViewModel viewModel)
            {
                // pridam do selectedItems ty radky, ktere maji vyplnene "Optimalizace"
                var selected = viewModel.EmployeeDetails.Where(d => d.Selected).ToList();
                selected.ForEach(d => AdwDataGrid.SelectedItems.Add(d));
                
                // napojim event na jednotlivych radcich () na event v teto tride - kazdy selectionChanged na radku vyvola event zde:
                 viewModel.EmployeeDetails.ForEach(d => d.SelectionChanged += RowSelectionChanged);
            }

            AdwDataGrid.SelectionChanged += ChangeSelection;


            // setnu filtr
            // var column = 
        }

        // jakekoli tlacitko (prikazani/zakazani bloku) na radku prileze sem:
        private void RowSelectionChanged(int id, bool? value)
        {
            if (AdwDataGrid.DataContext is ViewModel viewModel && value == false)
            {
                var employee = viewModel.EmployeeDetails.First(d => d.Id == id);
                employee.Reason = "Uzivatel";
            }
        }

        private void ChangeSelection(object sender, GridSelectionChangedEventArgs e)
        {
            if (sender is SfDataGrid sfDataGrid)
            {
                var rows = e.AddedItems;
            }
        }
        
        
        
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var other = new OtherWindow();
            other.ShowDialog();
        }
    }
}
