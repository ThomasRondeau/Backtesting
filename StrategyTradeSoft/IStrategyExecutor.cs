using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTradeSoft
{
    public interface IStrategyExecutor
    {
        OrderService orderService { get; init; }
        public void RunPortfolio() { }
        public void AddProduct(Product product) { }

    }
}