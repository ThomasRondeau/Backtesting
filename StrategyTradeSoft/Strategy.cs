using System;
using System.Collections.Generic;
using IndicatorsApp.Indicators;

namespace StrategyTradeSoft
{
    public abstract class Strategy
    {
        public int Id { get; private set; }
        protected static int IdCounter = 0;

        protected List<Indicators> IndicatorsList = new List<Indicators>();

        protected DateOnly startDate;

        protected DateOnly endDate;

        protected double Notional;

        protected List<string> LogMessages = new List<string>();

        public Strategy(List<Indicators> indicators, DateOnly startDate, DateOnly endDate, double notional)
        {
            Id = IdCounter++;
            IndicatorsList = indicators;
            this.startDate = startDate;
            this.endDate = endDate;
            Notional = notional;
        }

        public abstract void Next(Tick tick);

        protected void Log(string message)
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