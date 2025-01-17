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
        private IndicatorCache<double> shortEMACache;
        private IndicatorCache<double> longEMACache;
        private IndicatorCache<double> macdCache;
        private IndicatorCache<double> signalLineCache;
        public List<double> MACDValues { get; private set; } // Modifié pour être public
        public List<double> SignalLine { get; private set; } // Modifié pour être public

        public MACD(int shortPeriod, int longPeriod, int signalPeriod)
        {
            this.shortPeriod = shortPeriod;
            this.longPeriod = longPeriod;
            this.signalPeriod = signalPeriod;
            shortEMACache = new IndicatorCache<double>(shortPeriod);
            longEMACache = new IndicatorCache<double>(longPeriod);
            macdCache = new IndicatorCache<double>(signalPeriod);
            signalLineCache = new IndicatorCache<double>(signalPeriod);
            MACDValues = new List<double>();
            SignalLine = new List<double>();
        }

        public override void Calculate(List<double> data)
        {
            double shortEMA = 0;
            double longEMA = 0;
            double macd = 0;
            double signal = 0;

            foreach (var price in data)
            {
                shortEMACache.Add(price);
                longEMACache.Add(price);

                if (shortEMACache.GetAll().Length >= shortPeriod)
                {
                    shortEMA = shortEMACache.GetAll().Average();
                }

                if (longEMACache.GetAll().Length >= longPeriod)
                {
                    longEMA = longEMACache.GetAll().Average();
                }

                if (shortEMACache.GetAll().Length >= shortPeriod && longEMACache.GetAll().Length >= longPeriod)
                {
                    macd = shortEMA - longEMA;
                    macdCache.Add(macd);
                    MACDValues.Add(macd);

                    if (macdCache.GetAll().Length >= signalPeriod)
                    {
                        signal = macdCache.GetAll().Average();
                        signalLineCache.Add(signal);
                        SignalLine.Add(signal);
                    }
                }
            }
        }

        public override void Display()
        {
            Console.Write("MACD Values: ");
            foreach (var macd in MACDValues)
            {
                Console.Write(macd + " ");
            }
            Console.WriteLine();

            Console.Write("Signal Line: ");
            foreach (var signal in SignalLine)
            {
                Console.Write(signal + " ");
            }
            Console.WriteLine();
        }
    }
}
