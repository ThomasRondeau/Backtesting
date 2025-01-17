namespace TestAlpha;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;

public class DataLoader
{
    private string _apiKey;
    private RestClient _client;

    public bool Verbose { get; set; } // Enables or disables verbose output

    // Initialize the data loader without API key
    public DataLoader(bool verbose = false)
    {
        _client = new RestClient("https://www.alphavantage.co");
        Verbose = verbose;
    }

    // Initialize the data loader with API key
    public void Initialize(string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));
        }

        _apiKey = apiKey;
        _client = new RestClient("https://www.alphavantage.co");

        if (Verbose)
        {
            Console.WriteLine("API key initialized successfully.");
        }
    }

    // Initialize the data loader with API key from environment variables
    public void InitializeFromEnvironment()
    {
        var apiKey = Environment.GetEnvironmentVariable("ALPHAVANTAGE_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("API key not found in environment variables. Please set the 'ALPHAVANTAGE_API_KEY' variable.");
        }

        _apiKey = apiKey;
        _client = new RestClient("https://www.alphavantage.co");

        if (Verbose)
        {
            Console.WriteLine("API key initialized from environment variables.");
        }
    }

    // Fetch data for a specific stock over a given period
    public async Task<JObject> GetStockDataAsync(string symbol, DateTime startDate, DateTime endDate, string interval = "daily", string outputSize = "compact")
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new InvalidOperationException("API key is not initialized. Call Initialize() with a valid API key.");
        }

        var request = new RestRequest("/query", Method.Get);
        request.AddParameter("function", "TIME_SERIES_" + interval.ToUpper());
        request.AddParameter("symbol", symbol);
        request.AddParameter("apikey", _apiKey);
        request.AddParameter("outputsize", outputSize);

        if (Verbose)
        {
            Console.WriteLine($"Fetching data for symbol: {symbol}, Interval: {interval}, Output Size: {outputSize}");
        }

        var response = await _client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            throw new Exception($"Error fetching data from Alpha Vantage: {response.StatusDescription}");
        }

        if (Verbose)
        {
            Console.WriteLine("Data fetched successfully.");
        }

        var data = JObject.Parse(response.Content);

        if (Verbose)
        {
            Console.WriteLine("Response Content:");
            Console.WriteLine(response.Content);

            Console.WriteLine("Available keys in the response:");
            foreach (var key in data.Properties())
            {
                Console.WriteLine(key.Name);
            }
        }

        var timeSeriesKey = data.Properties().FirstOrDefault(p => p.Name.StartsWith("Time Series"))?.Name;

        if (string.IsNullOrEmpty(timeSeriesKey))
        {
            throw new Exception("Time series data not found in the response.");
        }

        var timeSeriesData = data[timeSeriesKey] as JObject;



        if (timeSeriesData == null)
        {
            throw new Exception("Time series data object is null. Verify the response structure.");
        }

        

        return timeSeriesData;
    }

    // Save the data to a CSV file
    public void SaveDataToCsv(JObject data, string filePath)
    {
        if (data == null || !data.HasValues)
        {
            throw new ArgumentException("Data is null or empty.", nameof(data));
        }

        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Date,Open,High,Low,Close");

            foreach (var property in data.Properties())
            {
                var date = property.Name;
                var details = property.Value as JObject;

                var open = details?["1. open"]?.ToString() ?? "N/A";
                var high = details?["2. high"]?.ToString() ?? "N/A";
                var low = details?["3. low"]?.ToString() ?? "N/A";
                var close = details?["4. close"]?.ToString() ?? "N/A";

                writer.WriteLine($"{date},{open},{high},{low},{close}");
            }
        }

        Console.WriteLine($"Data saved successfully to: {filePath}");

        if (Verbose)
        {
            Console.WriteLine("Saved data:");
            Console.WriteLine(File.ReadAllText(filePath));
        }
    }
}
