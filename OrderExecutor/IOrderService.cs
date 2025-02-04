﻿namespace OrderExecutor.Classes
{
    public interface IOrderService
    {
        IEnumerable<Position> GetAllPositions();

        void Buy(int productId, string productName, double price, int quantity, DateTime time);

        void Sell(int productId, string productName, double price, int quantity, DateTime time);
    }
}
