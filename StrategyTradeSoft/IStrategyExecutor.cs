using OrderExecutor.Classes;

namespace StrategyTradeSoft
{
    public interface IStrategyExecutor
    {
        OrderService orderService { get; init; }
        public void RunPortfolio() { }
        public void AddProduct(Product product) { }

    }
}