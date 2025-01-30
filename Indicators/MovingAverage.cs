using System;
using System.Collections.Generic;
using System.Linq;

namespace IndicatorsApp.Indicators
{
    public class MovingAverage : Indicators
    {
        private int period;
        private IndicatorCache<double> cache;
        public List<double> Values { get; private set; }
        public double LastValue { get; private set; }
        public MovingAverage(int period)
        {
            this.period = period;
            cache = new IndicatorCache<double>(period);
            Values = new List<double>();
        }

        public override void Calculate(List<double> data)
        {
            foreach (var price in data)
            {
                cache.Add(price);
                if (cache.GetAll().Length >= period)
                {
                    double sum = cache.GetAll().Sum();
                    double ma = sum / period;
                    Values.Add(ma);
                    LastValue = ma;
                }
            }
        }

        public override void Display()
        {
            Console.Write("Moving Averages: ");
            foreach (var ma in Values)
            {
                Console.Write(ma + " ");
            }
            Console.WriteLine();
        }
    }
}