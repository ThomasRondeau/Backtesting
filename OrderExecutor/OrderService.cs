namespace OrderExecutor.Classes
{
    public class OrderService : IOrderService
    {
        private readonly Portfolio _portfolio;
        private readonly OrderExecutor _orderExecutor;

        public OrderService()
        {
            _portfolio = new Portfolio(10000000);
            _orderExecutor = new OrderExecutor(_portfolio);
        }

        public void Buy(double price, int quantity, DateTime time)
        {
            Order order = CreateOrder(OrderType.Buy, price, quantity, time);
            ExecuteOrder(order);
        }

        public void Sell(double price, int quantity, DateTime time)
        {
            Order order = CreateOrder(OrderType.Sell, price, quantity, time);
            ExecuteOrder(order);
        }

        public Order CreateOrder(OrderType type, double price, int quantity, DateTime time)
        {
            return new Order(type, price, quantity, time);
        }

        public void ExecuteOrder(Order order)
        {
            _orderExecutor.ProcessOrder(order);
        }
        public void GetPortfolioSummary()
        {
            _portfolio.PrintPortfolioSummary();
        }

        public IEnumerable<Position> GetOpenPositions()
        {
            throw new NotImplementedException("Nécessite l'accès aux positions dans la classe Portfolio");
        }

        public IEnumerable<Position> GetAllPositions()
        {
            throw new NotImplementedException("Nécessite l'accès aux positions dans la classe Portfolio");
        }

        public double GetAvailableCash()
        {
            throw new NotImplementedException("Nécessite l'accès au cash dans la classe Portfolio");
        }

        public bool CanExecuteOrder(Order order)
        {
            if (order.Type == OrderType.Buy)
            {
                double requiredCash = order.Price * order.Quantity;
                return GetAvailableCash() >= requiredCash;
            }
            // Pour un ordre de vente, vérifie s'il y a une position ouverte correspondante
            return true; // À implémenter quand accès aux positions sera disponible
        }
    }
}
