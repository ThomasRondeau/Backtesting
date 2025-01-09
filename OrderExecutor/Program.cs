using System;
using System.Collections.Generic;

namespace ForexOrderExecutor
{
    public enum OrderType
    {
        Buy,
        Sell
    }

    public class Order
    {
        public OrderType Type { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime Time { get; set; }

        public Order(OrderType type, double price, int quantity, DateTime time)
        {
            Type = type;
            Price = price;
            Quantity = quantity;
            Time = time; 
        }
    }

    public class Position
    {
        public OrderType Type { get; private set; }
        public double EntryPrice { get; private set; }
        public int Quantity { get; private set; }
        public DateTime EntryTime { get; private set; }
        public double? ExitPrice { get; private set; }
        public DateTime? ExitTime { get; private set; }

        public double ProfitLoss => ExitPrice.HasValue
            ? (ExitPrice.Value - EntryPrice) * Quantity * (Type == OrderType.Buy ? 1 : -1)
            : 0;

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
    }

    public class Portfolio
    {
        private List<Position> _positions = new List<Position>();
        private double _cash;

        public Portfolio(double initialCash)
        {
            _cash = initialCash;
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
            if (_cash >= cost)
            {
                _positions.Add(new Position(OrderType.Buy, order.Price, order.Quantity, order.Time));
                _cash -= cost;
                Console.WriteLine($"Opened BUY position: {order.Quantity} units at {order.Price}, remaining cash: {_cash:F2}");
            }
            else
            {
                Console.WriteLine("Insufficient funds to open position.");
            }
        }

        private void ClosePosition(Order order)
        {
            foreach (var position in _positions)
            {
                if (position.Type == OrderType.Buy && !position.ExitPrice.HasValue)
                {
                    position.Close(order.Price, order.Time);
                    _cash += order.Price * position.Quantity;
                    Console.WriteLine($"Closed BUY position at {order.Price}, P&L: {position.ProfitLoss:F2}, total cash: {_cash:F2}");
                    break;
                }
            }
        }

        public void PrintPortfolioSummary()
        {
            Console.WriteLine("\nPortfolio Summary:");
            Console.WriteLine($"Cash: {_cash:F2}");
            foreach (var position in _positions)
            {
                string status = position.ExitPrice.HasValue ? "Closed" : "Open";
                Console.WriteLine(
                    $"- {status} {position.Type} position: {position.Quantity} units at {position.EntryPrice} (P&L: {position.ProfitLoss:F2})");
            }
        }
    }

    // Class OrderExecutor
    public class OrderExecutor
    {
        private Portfolio _portfolio;

        public OrderExecutor(Portfolio portfolio)
        {
            _portfolio = portfolio;
        }

        public void ProcessOrder(Order order)
        {
            _portfolio.ExecuteOrder(order);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Initialise the portfolio with a starting capital
            var portfolio = new Portfolio(10000);
            var executor = new OrderExecutor(portfolio);

            // Example of order
            var orders = new List<Order>
            {
                new Order(OrderType.Buy, 1.1200, 1000, DateTime.Now),
                new Order(OrderType.Sell, 1.1250, 1000, DateTime.Now.AddHours(1)),
                new Order(OrderType.Buy, 1.1180, 1000, DateTime.Now.AddHours(2)),
                new Order(OrderType.Sell, 1.1220, 1000, DateTime.Now.AddHours(3))
            };

            // load orders from csv file
            //var orders = LoadOrdersFromCsv("orders.csv");

            // processing orders
            foreach (var order in orders)
            {
                executor.ProcessOrder(order);
            }

            // Resume portfolio
            portfolio.PrintPortfolioSummary();
        }
    }
}
