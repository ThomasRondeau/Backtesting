namespace OrderExecutor.Classes
{
    public class OrderService : IOrderService
    {
        private readonly Portfolio _portfolio;
        private readonly OrderExecutor _orderExecutor;

        public OrderService(double initialCash)
        {
            _portfolio = new Portfolio(initialCash);
            _orderExecutor = new OrderExecutor(_portfolio);
        }

        public Order CreateOrder(OrderType type, double price, int quantity, DateTime time)
        {
            return new Order(type, price, quantity, time);
        }

        public void ExecuteOrder(Order order)
        {
            if (CanExecuteOrder(order))
            {
                _orderExecutor.ProcessOrder(order);
            }
            else
            {
                throw new InvalidOperationException("L'ordre ne peut pas être exécuté en raison de fonds insuffisants ou de conditions non remplies.");
            }
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
