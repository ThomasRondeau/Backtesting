using System;
using System.Collections.Generic;

namespace IndicatorsApp.Indicators
{
    public abstract class Indicators
    {
        public List<double> Values { get; private set; }
        public double LastValue { get; private set; }
        public abstract void Calculate(List<double> data);
        public abstract void Display();
    }
}
