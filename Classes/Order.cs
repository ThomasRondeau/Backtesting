using System;

namespace StrategyTradeSoft.Classes
{
    using StrategyTradeSoft.Services;

    public class MovingAverageCrossover : Strategy
    {
        private List<float> Prices = new List<float>();
        private int ShortPeriod;
        private int LongPeriod;
        private float LastShortMA;
        private float LastLongMA;

        public MovingAverageCrossover(int shortPeriod, int longPeriod)
        {
            ShortPeriod = shortPeriod;
            LongPeriod = longPeriod;
        }

        public override void Next(Tick tick)
        {
            Prices.Add(tick.Price);

            if (Prices.Count >= LongPeriod)
            {
                float shortMA = IndicatorService.CalculateSMA(Prices, ShortPeriod);
                float longMA = IndicatorService.CalculateSMA(Prices, LongPeriod);

                if (LastShortMA <= LastLongMA && shortMA > longMA)
                {
                    Log($"Buy signal at {tick.Time} - Price: {tick.Price}");
                }
                else if (LastShortMA >= LastLongMA && shortMA < longMA)
                {
                    Log($"Sell signal at {tick.Time} - Price: {tick.Price}");
                }

                LastShortMA = shortMA;
                LastLongMA = longMA;
            }
        }
    }
}
