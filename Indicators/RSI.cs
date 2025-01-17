using System;
using System.Collections.Generic;
using System.Linq;

namespace IndicatorsApp.Indicators
{
    public class RSI : Indicators
    {
        private int period;
        private IndicatorCache<double> gainsCache;
        private IndicatorCache<double> lossesCache;
        public List<double> RSIValues { get; private set; } // Modifié pour être public

        public RSI(int period)
        {
            this.period = period;
            gainsCache = new IndicatorCache<double>(period);
            lossesCache = new IndicatorCache<double>(period);
            RSIValues = new List<double>();
        }

        public override void Calculate(List<double> data)
        {
            for (int i = 1; i < data.Count; i++)
            {
                double change = data[i] - data[i - 1];
                if (change > 0)
                {
                    gainsCache.Add(change);
                    lossesCache.Add(0);
                }
                else
                {
                    gainsCache.Add(0);
                    lossesCache.Add(-change);
                }

                if (gainsCache.GetAll().Length >= period && lossesCache.GetAll().Length >= period)
                {
                    double avgGain = gainsCache.GetAll().Average();
                    double avgLoss = lossesCache.GetAll().Average();
                    double rs = avgGain / avgLoss;
                    double rsi = 100 - (100 / (1 + rs));
                    RSIValues.Add(rsi);
                }
            }
        }

        public override void Display()
        {
            Console.Write("RSI Values: ");
            foreach (var rsi in RSIValues)
            {
                Console.Write(rsi + " ");
            }
            Console.WriteLine();
        }
    }
}
