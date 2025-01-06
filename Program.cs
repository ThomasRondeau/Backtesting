using StrategyTradeSoft.Classes;
using StrategyTradeSoft.Services;
using StrategyTradeSoft.Strategies;

class Program
{
    static void Main(string[] args)
    {
        string filePath = @"C:\Users\marco\Documents\Project_Commando_C#\StrategyTradeSoft\StrategyTradeSoft\Data\EURUSD.csv";

        try
        {
            
            var ticks = DataLoader.LoadTicks(filePath);

            
            Strategy myStrategy = new MovingAverageCrossoverStrategy(); 

            
            foreach (var tick in ticks)
            {
                myStrategy.Next(tick);
            }

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
