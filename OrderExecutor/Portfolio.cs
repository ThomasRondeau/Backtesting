namespace OrderExecutor.Classes
{
    public class Portfolio
    {
        public List<Position> _positions = new List<Position>();
        public IReadOnlyList<Position> Positions => _positions;

        public List<double> PortfolioProfitLoss = new List<double>();
        public Portfolio()
        {
        }

        public void ExecuteOrder(Order order)
        {
            if (order.Type == OrderType.Buy)
            {
                OpenPosition(order);
            }
            else if (order.Type == OrderType.Sell)
            {
                ClosePosition(order);
            }
        }

        private void OpenPosition(Order order)
        {
            double cost = order.Price * order.Quantity;
            _positions.Add(new Position(OrderType.Buy, order.Price, order.Quantity, order.Time));
            Console.WriteLine($"Opened BUY position: {order.Quantity} units at {order.Price}");
        }

        private void ClosePosition(Order order)
        {
            foreach (var position in _positions)
            {
                if (position.Type == OrderType.Buy && !position.ExitPrice.HasValue)
                {
                    position.Close(order.Price, order.Time);
                    Console.WriteLine($"Closed BUY position at {order.Price}, P&L: {position.ProfitLoss:F2}");
                    break;
                }
            }
        }

        public void UpdateProfitLoss(double price)
        {
            double temp = 0;
            foreach(var position in _positions)
            {
                UpdateProfitLoss(price);
                temp += position.ProfitLoss.Last();
            }
            PortfolioProfitLoss.Add(temp);
        }

        public void PrintPortfolioSummary()
        {
            Console.WriteLine("\nPortfolio Summary:");
            foreach (var position in _positions)
            {
                string status = position.ExitPrice.HasValue ? "Closed" : "Open";
                Console.WriteLine(
                    $"- {status} {position.Type} position: {position.Quantity} units at {position.EntryPrice} (P&L: {position.ProfitLoss:F2})");
            }
        }
    }
}
