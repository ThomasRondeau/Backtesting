using System;
using System.Collections.Generic;

namespace Backtesting.Indicators
{
    public class OBV : Indicators
    {
        private IndicatorCache<double> cache;
        public List<double> OBVValues { get; private set; } // Modifié pour être public

        public OBV()
        {
            cache = new IndicatorCache<double>(1);
            OBVValues = new List<double>();
        }

        public override void Calculate(List<double> data)
        {
            double obv = 0;
            for (int i = 1; i < data.Count; i++)
            {
                cache.Add(data[i]);
                if (data[i] > data[i - 1])
                {
                    obv += data[i];
                }
                else if (data[i] < data[i - 1])
                {
                    obv -= data[i];
                }

                OBVValues.Add(obv);
            }
        }

        public override void Display()
        {
            Console.Write("OBV Values: ");
            foreach (var obv in OBVValues)
            {
                Console.Write(obv + " ");
            }
            Console.WriteLine();
        }
    }
}
