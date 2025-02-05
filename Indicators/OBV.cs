using System;
using System.Collections.Generic;

namespace IndicatorsApp.Indicators
{
    public class OBV : Indicators
    {
        private IndicatorCache<double> cache;

        public OBV()
        {
            cache = new IndicatorCache<double>(1);
            Values = new List<double>();
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

                Values.Add(obv);
                LastValue = obv;
            }
        }

        public override void Display()
        {
            Console.Write("OBV Values: ");
            foreach (var obv in Values)
            {
                Console.Write(obv + " ");
            }
            Console.WriteLine();
        }
    }
}
