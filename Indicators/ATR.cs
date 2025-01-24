using System;
using System.Collections.Generic;
using System.Linq;

namespace IndicatorsApp.Indicators
{
    public class ATR : Indicators
    {
        private int period;
        private IndicatorCache<double> cache;
        public List<double> ATRValues { get; private set; } // Modifié pour être public

        public ATR(int period)
        {
            this.period = period;
            cache = new IndicatorCache<double>(period);
            ATRValues = new List<double>();
        }

        public override void Calculate(List<double> data)
        {
            foreach (var price in data)
            {
                cache.Add(price);
                if (cache.GetAll().Length >= period)
                {
                    double highLow = Math.Abs(price - cache.GetAll().First());
                    double highClose = Math.Abs(price - cache.GetAll().First());
                    double lowClose = Math.Abs(cache.GetAll().First() - cache.GetAll().First());
                    double trueRange = Math.Max(highLow, Math.Max(highClose, lowClose));
                    double atr = cache.GetAll().Select(x => trueRange).Average();
                    ATRValues.Add(atr);
                }
            }
        }

        public override void Display()
        {
            Console.Write("ATR Values: ");
            foreach (var atr in ATRValues)
            {
                Console.Write(atr + " ");
            }
            Console.WriteLine();
        }
    }
}
