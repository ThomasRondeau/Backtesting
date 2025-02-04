using System;
using System.Collections.Generic;
using IndicatorsApp.Indicators;

namespace StrategyTradeSoft
{
    public class Product
    {
        private static int IdCounter = 0;
        public int Id { get; private set; }
        public string Name { get; private set; }
        public double Notional { get; set; }
        public List<Condition> Conditions { get; set; }
        public string CurrencyPair { get; set; }        

        public Product(string name, double notional, List<Condition> conditions)
        {
            Id = IdCounter++;
            Name = name;
            Notional = notional;
            Conditions = conditions;
        }
    }
}
