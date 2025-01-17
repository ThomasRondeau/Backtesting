using System;
using System.Collections.Generic;
using System.Linq;

namespace IndicatorsApp.Indicators
{
    public class RSI : Indicators
    {
        private int period;
        private List<double> rsiValues;

        public RSI(int period)
        {
            this.period = period;
            rsiValues = new List<double>();
        }

        public override void Calculate(List<double> data)
        {
            rsiValues.Clear();
            List<double> gains = new List<double>(new double[period]);
            List<double> losses = new List<double>(new double[period]);

            for (int i = 1; i < data.Count; i++)
            {
                double change = data[i] - data[i - 1];
                if (change > 0)
                {
                    gains.Add(change);
                    losses.Add(0);
                }
                else
                {
                    gains.Add(0);
                    losses.Add(-change);
                }

                if (i >= period)
                {
                    double avgGain = gains.Skip(i - period).Take(period).Average();
                    double avgLoss = losses.Skip(i - period).Take(period).Average();
                    double rs = avgGain / avgLoss;
                    rsiValues.Add(100 - (100 / (1 + rs)));
                }
            }
        }

        public override void Display()
        {
            Console.Write("RSI Values: ");
            foreach (var rsi in rsiValues)
            {
                Console.Write(rsi + " ");
            }
            Console.WriteLine();
        }
    }
}
