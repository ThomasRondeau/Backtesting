using System;
using System.Collections.Generic;

namespace Backtesting.Indicators
{
    public abstract class Indicators
    {
        public abstract void Calculate(List<double> data);
        public abstract void Display();
    }
}
