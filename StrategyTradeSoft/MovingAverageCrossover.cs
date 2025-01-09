using System;
using System.Collections.Generic;
using StrategyTradeSoft.Classes;

namespace StrategyTradeSoft.Strategies
{
    public partial class MovingAverageCrossoverStrategy : Strategy
    {
        private Queue<float> shortWindow = new Queue<float>();
        private Queue<float> longWindow = new Queue<float>();
        private int shortPeriod = 5;
        private int longPeriod = 20; 

        public override void Next(Tick tick)
        {
            shortWindow.Enqueue(tick.Price);
            longWindow.Enqueue(tick.Price);

            if (shortWindow.Count > shortPeriod) shortWindow.Dequeue();
            if (longWindow.Count > longPeriod) longWindow.Dequeue();

            if (shortWindow.Count == shortPeriod && longWindow.Count == longPeriod)
            {
                var shortAvg = CalculateAverage(shortWindow);
                var longAvg = CalculateAverage(longWindow);

                
                Console.WriteLine($"Tick Time: {tick.Time}, Short MA: {shortAvg:F5}, Long MA: {longAvg:F5}, Current Price: {tick.Price}");

                
                if (shortAvg > longAvg)
                {
                    Console.WriteLine($"Buy signal at {tick.Time} - Price: {tick.Price}. Reason: Short MA ({shortAvg:F5}) > Long MA ({longAvg:F5})");
                }
                else if (shortAvg < longAvg)
                {
                    Console.WriteLine($"Sell signal at {tick.Time} - Price: {tick.Price}. Reason: Short MA ({shortAvg:F5}) < Long MA ({longAvg:F5})");
                }
            }
        }

        private float CalculateAverage(Queue<float> window)
        {
            float sum = 0;
            foreach (var price in window)
            {
                sum += price;
            }
            return sum / window.Count;
        }
    }
}
