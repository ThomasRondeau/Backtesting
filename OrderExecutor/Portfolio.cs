using OrderExecutor.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderExecutor.Classes
{
    public partial class Portfolio
    {
        private List<Position> _positions = new List<Position>();
        private double _cash;
        public IReadOnlyList<Position> Positions => _positions;
        public double Cash => _cash;

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
}
