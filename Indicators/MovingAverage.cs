using System;
using System.Collections.Generic;
using System.Linq;

namespace IndicatorsApp.Indicators
{
    public class MovingAverage : Indicators
    {
        private int period;
        private IndicatorCache<double> cache;
        public List<double> MovingAverages { get; private set; } // Modifié pour être public

        public MovingAverage(int period)
        {
            this.period = period;
            cache = new IndicatorCache<double>(period);
            MovingAverages = new List<double>();
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
                    MovingAverages.Add(ma);
                }
            }
        }

        public override void Display()
        {
            Console.Write("Moving Averages: ");
            foreach (var ma in MovingAverages)
            {
                Console.Write(ma + " ");
            }
            Console.WriteLine();
        }
    }
}