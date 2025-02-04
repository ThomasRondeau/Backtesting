using Newtonsoft.Json.Linq;
using OrderExecutor.Classes;

namespace StrategyTradeSoft
{
    public interface IStrategyExecutor
    {
        IOrderService orderService { get; init; }

        public void addData(JObject jsonData);
        public void RunPortfolio() { }
        public void AddProduct(Product product) { }

    }
}