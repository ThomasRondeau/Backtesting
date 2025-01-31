using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderExecutor.Classes
{
    public class OrderExecutor
    {
        private Portfolio _portfolio;

        public OrderExecutor(Portfolio portfolio)
        {
            _portfolio = portfolio;
        }

        public void ProcessOrder(Order order)
        {
            _portfolio.ExecuteOrder(order);
        }
    }
}
