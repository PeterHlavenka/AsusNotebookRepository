#region Copyright Syncfusion Inc. 2001 - 2020
// Copyright Syncfusion Inc. 2001 - 2020. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.Windows.Shared;
using System;
using System.Windows;
using Syncfusion.SfSkinManager;

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

            var neco = Enum.GetValues(typeof(VisualStyles));
            var test = ComboVisualStyle.Items;
        } 
        
        
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var other = new OtherWindow();
            other.ShowDialog();
        }
    }
}
