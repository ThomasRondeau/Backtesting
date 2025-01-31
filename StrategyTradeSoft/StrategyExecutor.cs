using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTradeSoft
{
    public class StrategyExecutor : IStrategyExecutor
    {
        public List<Product> Products { get; private set; }

        public List<Tick> Data { get; private set; }
        //public OrderService orderService { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

        public void RunPortfolio()
        {
            foreach (var product in Products)
            {
                product.RunStrategy(Data); // Vérifier la data
            }
        }
        public void AddProduct(Product product) {
            Products.Add(product);
        }
    }
}
