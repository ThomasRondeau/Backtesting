namespace OrderExecutor.Classes
{
    public class Position
    {
        public OrderType Type { get; private set; }
        public int Quantity { get; private set; }
        public double EntryPrice { get; private set; }
        public DateTime EntryTime { get; private set; }
        public double? ExitPrice { get; private set; }
        public DateTime? ExitTime { get; private set; }

        public List<double> ProfitLoss { get; private set; }

        public Position(OrderType type, double entryPrice, int quantity, DateTime entryTime)
        {
            Type = type;
            EntryPrice = entryPrice;
            Quantity = quantity;
            EntryTime = entryTime;
        }

        public void Close(double exitPrice, DateTime exitTime)
        {
            ExitPrice = exitPrice;
            ExitTime = exitTime;
        }

        public void UpdatePNL(double price)
        {
            if (Type == OrderType.Buy)
            {
                ProfitLoss.Add((price - EntryPrice) * Quantity);
            }
            else
            {
                ProfitLoss.Add((EntryPrice - price) * Quantity);
            }
        }
    }
}