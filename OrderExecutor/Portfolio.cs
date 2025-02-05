namespace OrderExecutor.Classes
{
    public class Portfolio
    {
        private List<Position> _positions = new List<Position>();
        public IReadOnlyList<Position> Positions => _positions;

        public List<Position> ClosedPositions = new List<Position>();

        public List<double> PortfolioProfitLoss = new List<double>();
        public Portfolio()
        {
        }

        public void ExecuteOrder(Order order)
        {
            if (order.Type == OrderType.Buy)
            {
                List<Position> positions = _positions.Where(position => position.ProductId == order.ProductId && position.Type == OrderType.Sell).ToList();

                if (positions.Count() != 0)
                {
                    ClosePosition(positions.First(), order.Price, order.Time);
                } 
                else
                {
                    OpenPosition(order);
                }
            }
            else if (order.Type == OrderType.Sell)
            {
                List<Position> positions = _positions.Where(position => position.ProductId == order.ProductId && position.Type == OrderType.Buy).ToList();

                if (positions.Count() != 0)
                {
                    ClosePosition(positions.First(), order.Price, order.Time);
                } 
                else
                {
                    OpenPosition(order);
                }
            }
        }

        private void OpenPosition(Order order)
        {
            double cost = order.Price * order.Quantity;
            _positions.Add(new Position(order.ProductId, order.Type, order.Price, order.Quantity, order.Time));
            Console.WriteLine($"Opened {order.Type} position: {order.Quantity} units at {order.Price}");
        }
        
        private void ClosePosition(Position position, double price, DateTime time)
        {
            position.Close(price, time);
            Console.WriteLine($"Closed {position.Type} position: {position.Quantity} units at {price}");
            _positions.Remove(position);
            ClosedPositions.Add(position);
        }

        public void CloseAllPositions(double price, DateTime time)
        {
            foreach (var position in _positions)
            {
                ClosePosition(position, price, time);
            }
        }


        public void UpdateProfitLoss(double price)
        {
            double temp = 0;
            foreach (var position in _positions)
            {
                position.UpdatePNL(price);
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
