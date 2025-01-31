namespace OrderExecutor.Classes
{
    public interface IOrderService
    {
        // Create new order
        Order CreateOrder(OrderType type, double price, int quantity, DateTime time);

        void ExecuteOrder(Order order);

        void GetPortfolioSummary();

        IEnumerable<Position> GetOpenPositions();

        IEnumerable<Position> GetAllPositions();

        double GetAvailableCash();

        // Verify if we have enough cash to execute order
        bool CanExecuteOrder(Order order);
    }
}
