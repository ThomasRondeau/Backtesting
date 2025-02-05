using IndicatorsApp.Indicators;
using OrderExecutor.Classes;

namespace StrategyTradeSoft
{
    public class Condition
    {
        public Indicators Indicator { get; }
        public OrderType Type { get; }
        public double Value { get; }
        public bool IsOpenPosition { get; set; }

        public Condition(Indicators indicator, OrderType type, double value)
        {
            Indicator = indicator;
            Type = type;
            Value = value;
            IsOpenPosition = false;
        }
    }
}
