namespace StrategyTradeSoft
{
    public class Tick
    {
        public DateTime Time { get; set; }
        public double Price { get; set; }
        public int Volume { get; set; }

        public Tick(DateTime time, int volume, double price)
        {
            Time = time;
            Volume = volume;
            Price = price;
        }
    }
}
