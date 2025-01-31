using IndicatorsApp.Indicators;
using OrderExecutor.Classes;

namespace StrategyTradeSoft
{
    public class Condition
    {
        public Indicators Indicator { get; set; }
        public OrderType Type { get; set; }
        public double Value { get; set; }
    }
}
