using System;
using System.Collections.Generic;
using StrategyTradeSoft;
using OrderExecutor.Classes;

namespace OrderExecutor.Classes
{
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
            private MovingAverageCrossover _strategy;
            private List<double> _dailyReturns = new List<double>();
            private double _initialCash;
            private double _highestValue;

            public BacktestEngine(double initialCash)
            {
                _initialCash = initialCash;
                _portfolio = new Portfolio(initialCash);
                _orderExecutor = new OrderExecutor(_portfolio);
                _strategy = new MovingAverageCrossover();
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
