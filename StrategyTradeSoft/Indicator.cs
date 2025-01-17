using System;
using System.Collections.Generic;
using System.Linq;

namespace StrategyTradeSoft
{
    public static class Indicator
    {
        public static float CalculateSMA(List<float> prices, int period)
        {
            if (prices.Count < period) return 0;
            return prices.Skip(prices.Count - period).Take(period).Average();
        }

        public static float CalculateEMA(List<float> prices, int period)
        {
            if (prices.Count < period) return 0;

            float multiplier = 2f / (period + 1);
            float ema = prices.Take(period).Average();
            foreach (var price in prices.Skip(period))
            {
                ema = (price - ema) * multiplier + ema;
            }
            return ema;
        }
    }
}
