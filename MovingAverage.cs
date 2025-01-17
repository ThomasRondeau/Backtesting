using System;
using System.Collections.Generic;
using System.Linq;

namespace IndicatorsApp.Indicators
{
    public class MovingAverage : Indicators
    {
        private int period;
        private List<double> movingAverages;

        public MovingAverage(int period)
        {
            this.period = period;
            movingAverages = new List<double>();
        }

        public override void Calculate(List<double> data)
        {
            movingAverages.Clear();
            for (int i = 0; i < data.Count; i++)
            {
                if (i >= period - 1)
                {
                    double sum = data.Skip(i - period + 1).Take(period).Sum();
                    movingAverages.Add(sum / period);
                }
            }
        }

        public override void Display()
        {
            Console.Write("Moving Averages: ");
            foreach (var ma in movingAverages)
            {
                Console.Write(ma + " ");
            }
            Console.WriteLine();
        }
    }
}
