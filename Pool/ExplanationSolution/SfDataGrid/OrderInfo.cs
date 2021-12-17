namespace SfDataGrid
{
    public class OrderInfo
    {
        public OrderInfo(int orderId, string customerName, string country, string customerId, string shipCity)
        {
            OrderId = orderId;
            CustomerName = customerName;
            Country = country;
            CustomerId = customerId;
            ShipCity = shipCity;
        }

        public int OrderId { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Country { get; set; }

        public string ShipCity { get; set; }
    }
}