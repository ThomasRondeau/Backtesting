namespace StrategyTradeSoft
{
    public class Tick
    {
        public DateTime Time { get; set; }
        public double Price { get; set; }
        public string Symbol { get; set; }
        public int Volume { get; set; }

        public Tick(DateTime time, string symbol, int volume, double price)
        {
            Time = time;
            Symbol = symbol;
            Volume = volume;
            Price = price;
        }
    }
}
