using System;
using System.Collections.Generic;
using IndicatorsApp.Indicators;

namespace StrategyTradeSoft
{
    public class Product : IProduct
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public double Notional { get; set; } // Removed Investment, keeping Notional
        public List<Indicators> IndicatorsList { get; set; }
        public string CurrencyPair { get; set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        private List<string> LogMessages = new List<string>();

        public Product(int id, string name, double notional, string currencyPair,
                       List<Indicators> indicators, DateOnly startDate, DateOnly endDate)
        {
            Id = id;
            Name = name;
            Notional = notional;
            CurrencyPair = currencyPair;
            IndicatorsList = indicators;
            StartDate = startDate;
            EndDate = endDate;
        }

        public void ExecuteStrategy(List<Tick> data)
        {
            foreach (var tick in data)
            {
                foreach (var indicator in IndicatorsList)
                {
                    indicator.Calculate(new List<double> { tick.Price });
                }
                Log($"Processed tick at {tick.Time} with price {tick.Price}");
            }
        }

        public void RunStrategy(List<Tick> data)
        {
            Console.WriteLine($"Running strategy for {Name} with {Notional} in {CurrencyPair}");
            ExecuteStrategy(data); 
        }

        private void Log(string message)
        {
            LogMessages.Add($"{DateTime.Now}: {message}");
            Console.WriteLine(message);
        }

        public void SaveLogs(string filePath)
        {
            System.IO.File.WriteAllLines(filePath, LogMessages);
        }
    }
}
