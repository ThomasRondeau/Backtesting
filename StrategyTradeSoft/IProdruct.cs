using System;
using System.Collections.Generic;
using IndicatorsApp.Indicators;

namespace StrategyTradeSoft
{
    public interface IProduct
    {
        int Id { get; }
        double Notional { get; set; }
        List<Indicators> IndicatorsList { get; set; }
        string CurrencyPair { get; set; }

        void RunStrategy();
    }
}
