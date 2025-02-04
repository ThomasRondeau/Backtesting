using IndicatorsApp.Indicators;
using OrderExecutor.Classes;

namespace StrategyTradeSoft
{
    public readonly struct Condition
    {
        public Indicators Indicator { get; }
        public OrderType Type { get; }
        public double Value { get; }

        public Condition(Indicators indicator, OrderType type, double value)
        {
            Indicator = indicator;
            Type = type;
            Value = value;
        }
    }
}
