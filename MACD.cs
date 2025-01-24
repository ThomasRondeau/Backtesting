using System;
using System.Collections.Generic;
using System.Linq;

namespace IndicatorsApp.Indicators
{
    public class MACD : Indicators
    {
        private int shortPeriod;
        private int longPeriod;
        private int signalPeriod;
        private List<double> macdValues;
        private List<double> signalLine;

        public MACD(int shortPeriod, int longPeriod, int signalPeriod)
        {
            this.shortPeriod = shortPeriod;
            this.longPeriod = longPeriod;
            this.signalPeriod = signalPeriod;
            macdValues = new List<double>();
            signalLine = new List<double>();
        }

        public override void Calculate(List<double> data)
        {
            macdValues.Clear();
            signalLine.Clear();

            List<double> shortEMA = new List<double>(new double[data.Count]);
            List<double> longEMA = new List<double>(new double[data.Count]);

            double kShort = 2.0 / (shortPeriod + 1);
            double kLong = 2.0 / (longPeriod + 1);

            for (int i = 0; i < data.Count; i++)
            {
                if (i == 0)
                {
                    shortEMA[i] = data[i];
                    longEMA[i] = data[i];
                }
                else
                {
                    shortEMA[i] = (data[i] - shortEMA[i - 1]) * kShort + shortEMA[i - 1];
                    longEMA[i] = (data[i] - longEMA[i - 1]) * kLong + longEMA[i - 1];
                }

                if (i >= longPeriod - 1)
                {
                    macdValues.Add(shortEMA[i] - longEMA[i]);
                }
            }

            double kSignal = 2.0 / (signalPeriod + 1);
            for (int i = 0; i < macdValues.Count; i++)
            {
                if (i == 0)
                {
                    signalLine.Add(macdValues[i]);
                }
                else
                {
                    signalLine.Add((macdValues[i] - signalLine[i - 1]) * kSignal + signalLine[i - 1]);
                }
            }
        }

        public override void Display()
        {
            Console.Write("MACD Values: ");
            foreach (var macd in macdValues)
            {
                Console.Write(macd + " ");
            }
            Console.WriteLine();

            Console.Write("Signal Line: ");
            foreach (var signal in signalLine)
            {
                Console.Write(signal + " ");
            }
            Console.WriteLine();
        }
    }
}
