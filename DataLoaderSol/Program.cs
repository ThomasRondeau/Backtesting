using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TestPolygon
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Initialize the ForexDataLoader
            var dataLoader = new ForexDataLoader(verbose: true);

            try
            {
                // Initialize the API key
                dataLoader.Initialize("nfVWzVENmPlB9tXk06WS7TLF7GANhAHo"); // Alternatively, use dataLoader.Initialize("Your_API_Key");

                // Define forex pair and date range
                string fromSymbol = "USD";
                string toSymbol = "EUR";
                DateTime startDate = new DateTime(2023, 1, 1);
                DateTime endDate = new DateTime(2023, 12, 31);

                // Fetch forex data
                Console.WriteLine("Fetching forex data...");
                JObject forexData = await dataLoader.GetForexDataAsync(fromSymbol, toSymbol, startDate, endDate);

                // Save data to a CSV file
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "ForexData.csv");
                dataLoader.SaveForexDataToCsv(forexData, filePath);

                Console.WriteLine($"Forex data saved to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}