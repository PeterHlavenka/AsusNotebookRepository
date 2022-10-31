#region

using System.Collections.ObjectModel;

#endregion

namespace SfDataGrid
{
    public class ViewModel
    {
        public ViewModel()
        {
            Orders = new ObservableCollection<OrderInfo>();
            GenerateOrders();
        }

        public ObservableCollection<OrderInfo> Orders { get; set; }

        private void GenerateOrders()
        {
            Orders.Add(new OrderInfo(1001, "Maria Anders", "Germany", "ALFKI", "Berlin"));
            Orders.Add(new OrderInfo(1001, "Ana Trujilo", "Mexico", "ANATR", "Mexico D.F."));
            Orders.Add(new OrderInfo(1003, "Antonio Moreno", "Mexico", "ANTON", "Mexico D.F."));
            Orders.Add(new OrderInfo(1004, "Thomas Hardy", "UK", "AROUT", "London"));
            Orders.Add(new OrderInfo(1005, "Christina Berglund", "Sweden", "BERGS", "Lula"));
            Orders.Add(new OrderInfo(1006, "Hanna Moos", "Germany", "BLAUS", "Mannheim"));
            Orders.Add(new OrderInfo(1007, "Frederique Citeaux", "France", "BLONP", "Strasbourg"));
            Orders.Add(new OrderInfo(1008, "Martin Sommer", "Spain", "BOLID", "Madrid"));
            Orders.Add(new OrderInfo(1009, "Laurence Lebihan", "France", "BONAP", "Marseille"));
            Orders.Add(new OrderInfo(1010, "Elizabeth Lincoln", "Canada", "BOTTM", "Tsawassen"));
        }
    }
}