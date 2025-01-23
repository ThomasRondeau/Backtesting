namespace TestPolygon;

using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class test
{
    public static async Task Main(string[] args) 
    {
        try
        {
            Console.WriteLine("[INFO] Starting Forex Data Fetch Example...");

            // Example parameters
            string fromSymbol = "USD";
            string toSymbol = "EUR";
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 1, 10);

            // Fetch data using the singleton instance
            JObject forexData = await ForexDataLoaderSingleton.FetchForexDataAsync(fromSymbol, toSymbol, startDate, endDate);

            // Display fetched data
            Console.WriteLine("[INFO] Forex data fetched successfully:");
            Console.WriteLine(forexData.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] An error occurred: {ex.Message}");
        }
    }
}