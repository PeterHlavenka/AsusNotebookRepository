namespace SfDataGridRoztahovaniFiltru
{
    public class OrderInfo
    {
        public OrderInfo(int orderId, string customerName, string country, string customerId, string shipCity, decimal amount)
        {
            OrderId = orderId;
            CustomerName = customerName;
            Country = country;
            CustomerId = customerId;
            ShipCity = shipCity;
            Amount = amount;
        }

        public int OrderId { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Country { get; set; }

        public string ShipCity { get; set; }
        
        public decimal Amount { get; set; }
    }
}

