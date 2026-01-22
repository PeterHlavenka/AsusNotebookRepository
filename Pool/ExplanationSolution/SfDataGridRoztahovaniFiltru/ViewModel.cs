using System.Collections.ObjectModel;

namespace SfDataGridRoztahovaniFiltru
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
            Orders.Add(new OrderInfo(1001, "Maria Anders", "Germany", "ALFKI", "Berlin", 1250.50m));
            Orders.Add(new OrderInfo(1002, "Ana Trujilo", "Mexico", "ANATR", "Mexico D.F.", 2340.75m));
            Orders.Add(new OrderInfo(1003, "Antonio Moreno", "Mexico", "ANTON", "Mexico D.F.", 890.25m));
            Orders.Add(new OrderInfo(1004, "Thomas Hardy", "UK", "AROUT", "London", 3450.00m));
            Orders.Add(new OrderInfo(1005, "Christina Berglund", "Sweden", "BERGS", "Lula", 1670.30m));
            Orders.Add(new OrderInfo(1006, "Hanna Moos", "Germany", "BLAUS", "Mannheim", 2120.80m));
            Orders.Add(new OrderInfo(1007, "Frederique Citeaux", "France", "BLONP", "Strasbourg", 3890.60m));
            Orders.Add(new OrderInfo(1008, "Martin Sommer", "Spain", "BOLID", "Madrid", 1450.90m));
            Orders.Add(new OrderInfo(1009, "Laurence Lebihan", "France", "BONAP", "Marseille", 4120.45m));
            Orders.Add(new OrderInfo(1010, "Elizabeth Lincoln", "Canada", "BOTTM", "Tsawassen", 2890.15m));
            Orders.Add(new OrderInfo(1011, "Victoria Ashworth", "UK", "BSBEV", "London", 1980.25m));
            Orders.Add(new OrderInfo(1012, "Patricio Simpson", "Argentina", "CACTU", "Buenos Aires", 3210.70m));
            Orders.Add(new OrderInfo(1013, "Francisco Chang", "Mexico", "CENTC", "Mexico D.F.", 890.00m));
            Orders.Add(new OrderInfo(1014, "Yang Wang", "Switzerland", "CHOPS", "Bern", 5670.85m));
            Orders.Add(new OrderInfo(1015, "Pedro Afonso", "Brazil", "COMMI", "Sao Paulo", 2340.50m));
        }
    }
}

