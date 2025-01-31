using Newtonsoft.Json.Linq;
using StrategyTradeSoft;

namespace UI
{
    record CacheData
    {
        public JObject data { get; init; }
        public List<Product> products { get; init; }

        public CacheData(JObject data = null, List<Product> products = null)
        {
            this.data = data ?? new JObject();
            this.products = products ?? new List<Product>();
        }
    }
}
