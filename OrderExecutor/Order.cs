using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderExecutor.Classes
{
    public enum OrderType
    {
        Buy,
        Sell
    }

    public class Order
    {
        public OrderType Type { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime Time { get; set; }

        public Order(OrderType type, double price, int quantity, DateTime time)
        {
            Type = type;
            Price = price;
            Quantity = quantity;
            Time = time;
        }
    }

}
