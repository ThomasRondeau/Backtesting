using System;
using System.Collections.Generic;
using IndicatorsApp.Indicators;

namespace StrategyTradeSoft
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public double Notional { get; set; }
        public List<Condition> Conditions { get; set; }
        public string CurrencyPair { get; set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        

        public Product(int id, string name, double notional, List<Condition> conditions, DateOnly startDate, DateOnly endDate)
        {
            Id = id;
            Name = name;
            Notional = notional;
            Conditions = conditions;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
