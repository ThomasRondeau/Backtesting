using System;
using System.Collections.Generic;
using IndicatorsApp.Indicators;  // Import MovingAverage

namespace StrategyTradeSoft.Strategies
{
    public class MovingAverageCrossover
    {
        private List<double> Prices = new List<double>();
        private MovingAverage ShortMA;
        private MovingAverage LongMA;

        public bool HasSignal { get; private set; }
        public string Signal { get; private set; } // No more SignalType

        public MovingAverageCrossover(int shortPeriod, int longPeriod)
        {
            ShortMA = new MovingAverage(shortPeriod);
            LongMA = new MovingAverage(longPeriod);
        }

        public void Next(Tick tick)
        {
            Prices.Add(tick.Price);
            ShortMA.Calculate(Prices);
            LongMA.Calculate(Prices);

            if (ShortMA.MovingAverages.Count > 1 && LongMA.MovingAverages.Count > 1)
            {
                double lastShortMA = ShortMA.MovingAverages[^1];
                double lastLongMA = LongMA.MovingAverages[^1];
                double prevShortMA = ShortMA.MovingAverages[^2];
                double prevLongMA = LongMA.MovingAverages[^2];

                if (prevShortMA <= prevLongMA && lastShortMA > lastLongMA)
                {
                    HasSignal = true;
                    Signal = "Buy";
                    Console.WriteLine($"Buy signal at {tick.Time} - Price: {tick.Price}");
                }
                else if (prevShortMA >= prevLongMA && lastShortMA < lastLongMA)
                {
                    HasSignal = true;
                    Signal = "Sell";
                    Console.WriteLine($"Sell signal at {tick.Time} - Price: {tick.Price}");
                }
                else
                {
                    HasSignal = false;
                }
            }
        }

        public void ClearSignal()
        {
            HasSignal = false;
        }
    }
}
