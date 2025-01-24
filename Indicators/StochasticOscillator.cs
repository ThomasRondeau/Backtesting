using System;
using System.Collections.Generic;
using System.Linq;

namespace IndicatorsApp.Indicators
{
    public class StochasticOscillator : Indicators
    {
        private int period;
        private IndicatorCache<double> cache;
        public List<double> KLine { get; private set; } // Modifié pour être public
        public List<double> DLine { get; private set; } // Modifié pour être public

        public StochasticOscillator(int period)
        {
            this.period = period;
            cache = new IndicatorCache<double>(period);
            KLine = new List<double>();
            DLine = new List<double>();
        }

        public override void Calculate(List<double> data)
        {
            foreach (var price in data)
            {
                cache.Add(price);
                if (cache.GetAll().Length >= period)
                {
                    double lowestLow = cache.GetAll().Min();
                    double highestHigh = cache.GetAll().Max();
                    double k = ((price - lowestLow) / (highestHigh - lowestLow)) * 100;
                    KLine.Add(k);
                }
            }

            DLine = KLine.Select((k, index) => index >= 2 ? KLine.Skip(index - 2).Take(3).Average() : 0).ToList();
        }

        public override void Display()
        {
            Console.Write("K Line: ");
            foreach (var k in KLine)
            {
                Console.Write(k + " ");
            }
            Console.WriteLine();

            Console.Write("D Line: ");
            foreach (var d in DLine)
            {
                Console.Write(d + " ");
            }
            Console.WriteLine();
        }
    }
}
