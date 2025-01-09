using System;
using System.Collections.Generic;
using StrategyTradeSoft;
using StrategyTradeSoft.Classes;

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

    public partial class Portfolio
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

   


        public class BacktestResult
        {
            public double TotalProfitLoss { get; set; }
            public int TotalTrades { get; set; }
            public int WinningTrades { get; set; }
            public int LosingTrades { get; set; }
            public double WinRate => TotalTrades > 0 ? (double)WinningTrades / TotalTrades * 100 : 0;
            public double MaxDrawdown { get; set; }
            public double SharpeRatio { get; set; }
            public List<Position> TradeHistory { get; set; } = new List<Position>();

            public void PrintResults()
            {
                Console.WriteLine("\n=== Backtest Results ===");
                Console.WriteLine($"Total P&L: {TotalProfitLoss:F2}");
                Console.WriteLine($"Total Trades: {TotalTrades}");
                Console.WriteLine($"Winning Trades: {WinningTrades}");
                Console.WriteLine($"Losing Trades: {LosingTrades}");
                Console.WriteLine($"Win Rate: {WinRate:F2}%");
                Console.WriteLine($"Max Drawdown: {MaxDrawdown:F2}%");
                Console.WriteLine($"Sharpe Ratio: {SharpeRatio:F2}");
            }
        }

        public class BacktestEngine
        {
            private Portfolio _portfolio;
            private OrderExecutor _orderExecutor;
            private MovingAverageCrossoverStrategy _strategy;
            private List<double> _dailyReturns = new List<double>();
            private double _initialCash;
            private double _highestValue;

            public BacktestEngine(double initialCash)
            {
                _initialCash = initialCash;
                _portfolio = new Portfolio(initialCash);
                _orderExecutor = new OrderExecutor(_portfolio);
                _strategy = new MovingAverageCrossoverStrategy();
                _highestValue = initialCash;
            }

            public void ProcessTick(Tick tick)
            {
                _strategy.Next(tick);

                if (_strategy.HasSignal)
                {
                    Order order = new Order(
                        _strategy.Signal == SignalType.Buy ? OrderType.Buy : OrderType.Sell,
                        tick.Price,
                        CalculatePositionSize(_portfolio, tick.Price),
                        tick.Time
                    );

                    _orderExecutor.ProcessOrder(order);
                    UpdateMetrics(tick);
                }
            }

            private void UpdateMetrics(Tick tick)
            {
                double currentValue = _portfolio.GetTotalValue(tick.Price);
                _dailyReturns.Add((currentValue - _highestValue) / _highestValue);

                if (currentValue > _highestValue)
                {
                    _highestValue = currentValue;
                }
            }

            private int CalculatePositionSize(Portfolio portfolio, double currentPrice)
            {
                // Position sizing basé sur un risque de 2% par trade
                double riskAmount = portfolio.GetTotalValue(currentPrice) * 0.02;
                return (int)(riskAmount / currentPrice);
            }

            public BacktestResult GetResults()
            {
                var result = new BacktestResult();
                result.TradeHistory = _portfolio.GetClosedPositions();
                result.TotalTrades = result.TradeHistory.Count;

                foreach (var trade in result.TradeHistory)
                {
                    result.TotalProfitLoss += trade.ProfitLoss;
                    if (trade.ProfitLoss > 0)
                        result.WinningTrades++;
                    else
                        result.LosingTrades++;
                }

                result.MaxDrawdown = CalculateMaxDrawdown();
                result.SharpeRatio = CalculateSharpeRatio();

                return result;
            }

            private double CalculateMaxDrawdown()
            {
                double maxDrawdown = 0;
                double peak = _initialCash;

                foreach (var dailyReturn in _dailyReturns)
                {
                    double currentValue = peak * (1 + dailyReturn);
                    if (currentValue > peak)
                        peak = currentValue;

                    double drawdown = (peak - currentValue) / peak * 100;
                    maxDrawdown = Math.Max(maxDrawdown, drawdown);
                }

                return maxDrawdown;
            }

            private double CalculateSharpeRatio()
            {
                if (_dailyReturns.Count == 0)
                    return 0;

                double averageReturn = _dailyReturns.Average();
                double stdDev = CalculateStandardDeviation(_dailyReturns);

                // Annualisé avec 252 jours de trading
                return stdDev != 0 ? (averageReturn / stdDev) * Math.Sqrt(252) : 0;
            }

            private double CalculateStandardDeviation(List<double> values)
            {
                if (values.Count <= 1)
                    return 0;

                double avg = values.Average();
                double sumOfSquares = values.Sum(val => Math.Pow(val - avg, 2));
                return Math.Sqrt(sumOfSquares / (values.Count - 1));
            }
        }

        // Extension de la classe Portfolio existante
        public partial class Portfolio
        {
            public double GetTotalValue(double currentPrice)
            {
                double positionsValue = _positions
                    .Where(p => !p.ExitPrice.HasValue)
                    .Sum(p => p.Quantity * currentPrice);
                return _cash + positionsValue;
            }

            public List<Position> GetClosedPositions()
            {
                return _positions.Where(p => p.ExitPrice.HasValue).ToList();
            }
        }

        // Extension de MovingAverageCrossoverStrategy
        /*public partial class MovingAverageCrossoverStrategy
        {
            public bool HasSignal { get; private set; }
            public SignalType Signal { get; private set; }

            public void Next(Tick tick)
            {
                // ... (votre code existant)

                if (shortAvg > longAvg && !HasSignal)
                {
                    HasSignal = true;
                    Signal = SignalType.Buy;
                }
                else if (shortAvg < longAvg && HasSignal)
                {
                    HasSignal = true;
                    Signal = SignalType.Sell;
                }
                else
                {
                    HasSignal = false;
                }
            }
        }*/

        public enum SignalType
        {
            Buy,
            Sell
        }
    
}
