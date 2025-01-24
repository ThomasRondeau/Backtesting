using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TestPolygon
{
    public static class ForexDataLoaderSingleton
    {
        private static ForexDataLoader _instance;
        private static readonly object _lock = new object();

        // Singleton Instance Property
        public static ForexDataLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ForexDataLoader(verbose: true);
                            _instance.InitializeFromEnvironment(); // Initialize API key from environment
                        }
                    }
                }
                return _instance;
            }
        }

        // FetchForexData wrapper function
        public static async Task<JObject> FetchForexDataAsync(string fromSymbol, string toSymbol, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (Instance.Verbose)
                {
                    Console.WriteLine("[INFO] Fetching forex data using Singleton instance.");
                }

                return await Instance.FetchForexData(fromSymbol, toSymbol, startDate, endDate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to fetch forex data: {ex.Message}");
                throw;
            }
        }
    }
}