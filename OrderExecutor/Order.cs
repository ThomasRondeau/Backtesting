namespace OrderExecutor.Classes
{
    public enum OrderType
    {
        Buy,
        Sell
    }

    public class Order
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public OrderType Type { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime Time { get; set; }

        public Order(int productId, string productName, OrderType type, double price, int quantity, DateTime time)
        {
            ProductId = productId;
            ProductName = productName;
            Type = type;
            Price = price;
            Quantity = quantity;
            Time = time;
        }
    }
}