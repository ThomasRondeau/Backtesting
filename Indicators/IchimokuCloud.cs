using System;
using System.Collections.Generic;
using System.Linq;

namespace IndicatorsApp.Indicators
{
    public class IchimokuCloud : Indicators
    {
        private int conversionPeriod;
        private int basePeriod;
        private int laggingSpanPeriod;
        private int displacement;
        private IndicatorCache<double> conversionCache;
        private IndicatorCache<double> baseCache;
        private IndicatorCache<double> laggingSpanCache;
        public List<double> ConversionLine { get; private set; } // Modifié pour être public
        public List<double> BaseLine { get; private set; } // Modifié pour être public
        public List<double> LeadingSpanA { get; private set; } // Modifié pour être public
        public List<double> LeadingSpanB { get; private set; } // Modifié pour être public
        public List<double> LaggingSpan { get; private set; } // Modifié pour être public

        public IchimokuCloud(int conversionPeriod, int basePeriod, int laggingSpanPeriod, int displacement)
        {
            this.conversionPeriod = conversionPeriod;
            this.basePeriod = basePeriod;
            this.laggingSpanPeriod = laggingSpanPeriod;
            this.displacement = displacement;
            conversionCache = new IndicatorCache<double>(conversionPeriod);
            baseCache = new IndicatorCache<double>(basePeriod);
            laggingSpanCache = new IndicatorCache<double>(laggingSpanPeriod);
            ConversionLine = new List<double>();
            BaseLine = new List<double>();
            LeadingSpanA = new List<double>();
            LeadingSpanB = new List<double>();
            LaggingSpan = new List<double>();
        }

        public override void Calculate(List<double> data)
        {
            foreach (var price in data)
            {
                conversionCache.Add(price);
                baseCache.Add(price);
                laggingSpanCache.Add(price);

                if (conversionCache.GetAll().Length >= conversionPeriod)
                {
                    double conversion = (conversionCache.GetAll().Max() + conversionCache.GetAll().Min()) / 2;
                    ConversionLine.Add(conversion);
                }

                if (baseCache.GetAll().Length >= basePeriod)
                {
                    double baseValue = (baseCache.GetAll().Max() + baseCache.GetAll().Min()) / 2;
                    BaseLine.Add(baseValue);
                }

                if (laggingSpanCache.GetAll().Length >= laggingSpanPeriod)
                {
                    double lagging = (laggingSpanCache.GetAll().Max() + laggingSpanCache.GetAll().Min()) / 2;
                    LaggingSpan.Add(lagging);
                }
            }

            for (int i = 0; i < Math.Min(ConversionLine.Count, BaseLine.Count); i++)
            {
                LeadingSpanA.Add((ConversionLine[i] + BaseLine[i]) / 2);
            }

            for (int i = 0; i < Math.Min(data.Count, LaggingSpan.Count); i++)
            {
                LeadingSpanB.Add((data[i] + LaggingSpan[i]) / 2);
            }
        }

        public override void Display()
        {
            Console.Write("Conversion Line: ");
            foreach (var cl in ConversionLine)
            {
                Console.Write(cl + " ");
            }
            Console.WriteLine();

            Console.Write("Base Line: ");
            foreach (var bl in BaseLine)
            {
                Console.Write(bl + " ");
            }
            Console.WriteLine();

            Console.Write("Leading Span A: ");
            foreach (var lsa in LeadingSpanA)
            {
                Console.Write(lsa + " ");
            }
            Console.WriteLine();

            Console.Write("Leading Span B: ");
            foreach (var lsb in LeadingSpanB)
            {
                Console.Write(lsb + " ");
            }
            Console.WriteLine();

            Console.Write("Lagging Span: ");
            foreach (var ls in LaggingSpan)
            {
                Console.Write(ls + " ");
            }
            Console.WriteLine();
        }
    }
}
