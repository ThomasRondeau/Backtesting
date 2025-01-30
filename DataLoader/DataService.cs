using System;
using System.Threading.Tasks;
using DataLoader;
using Newtonsoft.Json.Linq;

namespace TestPolygon
{
    public class DataService : IDataService
    {
        private bool Verbose { get; set; }
        private ForexDataLoader DataLoader;
        public DataService(bool verbose)
        {
            Verbose = verbose;
            DataLoader = new ForexDataLoader(verbose: true);
            DataLoader.InitializeFromEnvironment();
        }

        public async Task<JObject> FetchForexDataAsync(string fromSymbol, string toSymbol, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (Verbose)
                {
                    Console.WriteLine("[INFO] Fetching forex data using Singleton instance.");
                }

                return await DataLoader.FetchForexData(fromSymbol, toSymbol, startDate, endDate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to fetch forex data: {ex.Message}");
                throw;
            }
        }
    }
}