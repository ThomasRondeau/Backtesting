using System;
using System.Collections.Generic;

namespace IndicatorsApp.Indicators
{
    public abstract class Indicators
    {
        public List<double> Values { get; protected set; }
        public double LastValue { get; protected set; }
        public abstract void Calculate(List<double> data);
        public abstract void Display();
    }
}
