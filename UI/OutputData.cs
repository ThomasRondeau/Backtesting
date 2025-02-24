﻿using IndicatorsApp.Indicators;
using Newtonsoft.Json.Linq;
using OrderExecutor.Classes;

namespace UI
{
    struct OutputData
    {
        public JObject data { get; init; }
        public List<Indicators> indicators { get; init; }
        public Portfolio Portfolio { get; init; }

        public OutputData(JObject data = null, List<Indicators> indicators = null, Portfolio portfolio = null)
        {
            this.data = data ?? new JObject();
            this.indicators = indicators ?? new List<Indicators>();
            Portfolio = portfolio ?? new Portfolio();
        }
    }
}