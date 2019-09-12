using System;

namespace WpfUniverse.Gui.Events
{
  public  class DataGridLostFocus
    {
        private void DataGrid_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Console.WriteLine(@"LostFocus");
        }
    }
}
