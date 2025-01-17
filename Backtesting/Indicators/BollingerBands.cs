using System;
using System.Collections.Generic;
using System.Linq;

namespace Backtesting.Indicators
{
    public class BollingerBands : Indicators
    {
        private int period;
        private double standardDeviations;
        private IndicatorCache<double> cache;
        public List<double> MiddleBand { get; private set; } // Modifié pour être public
        public List<double> UpperBand { get; private set; } // Modifié pour être public
        public List<double> LowerBand { get; private set; } // Modifié pour être public

        public BollingerBands(int period, double standardDeviations)
        {
            this.period = period;
            this.standardDeviations = standardDeviations;
            cache = new IndicatorCache<double>(period);
            MiddleBand = new List<double>();
            UpperBand = new List<double>();
            LowerBand = new List<double>();
        }

        public override void Calculate(List<double> data)
        {
            foreach (var price in data)
            {
                cache.Add(price);
                if (cache.GetAll().Length >= period)
                {
                    double sma = cache.GetAll().Average();
                    double stdDev = Math.Sqrt(cache.GetAll().Select(x => Math.Pow(x - sma, 2)).Sum() / period);
                    MiddleBand.Add(sma);
                    UpperBand.Add(sma + standardDeviations * stdDev);
                    LowerBand.Add(sma - standardDeviations * stdDev);
                }
            }
        }

        public override void Display()
        {
            Console.Write("Middle Band: ");
            foreach (var mb in MiddleBand)
            {
                Console.Write(mb + " ");
            }
            Console.WriteLine();

            Console.Write("Upper Band: ");
            foreach (var ub in UpperBand)
            {
                Console.Write(ub + " ");
            }
            Console.WriteLine();

            Console.Write("Lower Band: ");
            foreach (var lb in LowerBand)
            {
                Console.Write(lb + " ");
            }
            Console.WriteLine();
        }
    }
}
