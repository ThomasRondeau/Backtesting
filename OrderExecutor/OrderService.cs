namespace OrderExecutor.Classes
{
    public class OrderService : IOrderService
    {
        private readonly Portfolio _portfolio;

        public OrderService()
        {
            _portfolio = new Portfolio();
        }

        public void Buy(int productId, string name, double price, int quantity, DateTime time)
        {
            Order order = new(productId, name, OrderType.Buy, price, quantity, time);
            _portfolio.ExecuteOrder(order);
        }

        public void Sell(int productId, string name, double price, int quantity, DateTime time)
        {
            Order order = new(productId, name, OrderType.Sell, price, quantity, time);
            _portfolio.ExecuteOrder(order);
        }

        public void CloseAllPositions(double price, DateTime time)
        {
            _portfolio.CloseAllPositions(price, time);
        }

        public IEnumerable<Position> GetAllPositions()
        {
            return _portfolio.Positions;
        }

        public Portfolio GetPortfolio()
        {
            return _portfolio;
        }

        public void UpdatePNL(double price)
        {
            _portfolio.UpdateProfitLoss(price);
        }
    }
}
