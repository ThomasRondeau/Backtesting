namespace TestPolygon;

using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Initialize the ForexDataLoader
            var dataLoader = new ForexDataLoader(verbose: true);

            // Reset the XML database
            Console.WriteLine("[INFO] Resetting the XML database...");
            dataLoader.ResetXML();

            // Initialize API key (use your own or set it in environment variables)
            dataLoader.Initialize("Op7aVxAltGZoKdjmiJaNzhrd9xeSZI6P");

            // Define forex pair and date range
            string fromSymbol = "USD";
            string toSymbol = "EUR";
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 1, 10);

            // Fetch forex data using FetchForexDataFromApi the first time
            Console.WriteLine("[INFO] Fetching forex data for the first time using FetchForexData");
            JObject forexDataFirst = await dataLoader.FetchForexData(fromSymbol, toSymbol, startDate, endDate);
            Console.WriteLine("[INFO] First fetch completed.");
            


            // Fetch forex data again using FetchForexData
            Console.WriteLine("[INFO] Fetching forex data for the second time using FetchForexData...");
            JObject forexDataSecond = await dataLoader.FetchForexData(fromSymbol, toSymbol, startDate, endDate);
            Console.WriteLine("[INFO] Second fetch completed.");

            Console.WriteLine("[INFO] Test completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] An error occurred: {ex.Message}");
        }
    }
}