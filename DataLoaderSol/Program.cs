using System;
using System.Threading.Tasks;

namespace TestAlpha
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var dataLoader = new DataLoader();
                dataLoader.Verbose = true;

                // Initialize using environment variable
                dataLoader.Initialize("MZGR3V41WWVL97H8");

                // Define stock symbol and date range
                string symbol = "AAPL"; // Apple stock
                DateTime startDate = new DateTime(2020, 1, 1);
                DateTime endDate = new DateTime(2021, 12, 31);

                // Fetch stock data
                var stockData = await dataLoader.GetStockDataAsync(symbol, startDate, endDate);
                Console.WriteLine("Stock data fetched successfully.");

                // Save data to CSV
                string filePath = "AAPL_StockData.csv";
                dataLoader.SaveDataToCsv(stockData, filePath);
                Console.WriteLine($"Data saved to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}