namespace TestPolygon;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;

public class ForexDataLoader
{
    private string _apiKey;
    private RestClient _client;

    public bool Verbose { get; set; } // Enables or disables verbose output

    // Initialize the data loader without API key
    public ForexDataLoader(bool verbose = false)
    {
        _client = new RestClient("https://api.polygon.io");
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
        _client = new RestClient("https://api.polygon.io");

        if (Verbose)
        {
            Console.WriteLine("API key initialized successfully.");
        }
    }

    // Initialize the data loader with API key from environment variables
    public void InitializeFromEnvironment()
    {
        var apiKey = Environment.GetEnvironmentVariable("POLYGON_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("API key not found in environment variables. Please set the 'POLYGON_API_KEY' variable.");
        }

        _apiKey = apiKey;
        _client = new RestClient("https://api.polygon.io");

        if (Verbose)
        {
            Console.WriteLine("API key initialized from environment variables.");
        }
    }

    // Fetch forex data for a specific pair over a given period
    public async Task<JObject> GetForexDataAsync(string fromSym, string toSym, DateTime startDate, DateTime endDate)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new InvalidOperationException("API key is not initialized. Call Initialize() with a valid API key.");
        }

        var requestUrl = $"/v2/aggs/ticker/C:{fromSym}{toSym}/range/1/day/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}";
        requestUrl += "?adjusted=true&sort=asc";

        var request = new RestRequest(requestUrl, Method.Get);
        request.AddParameter("apiKey", _apiKey);

        Console.WriteLine($"Request URL: https://api.polygon.io{requestUrl}");


        if (Verbose)
        {
            Console.WriteLine($"Fetching forex data for: {fromSym}/{toSym}, Start Date: {startDate:yyyy-MM-dd}, End Date: {endDate:yyyy-MM-dd}");
        }

        var response = await _client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            throw new Exception($"Error fetching data from Polygon API: {response.StatusDescription}");
        }

        if (Verbose)
        {
            Console.WriteLine("Data fetched successfully.");
            Console.WriteLine("Response Content:");
            Console.WriteLine(response.Content);
        }

        var data = JObject.Parse(response.Content);

        if (!data.ContainsKey("results"))
        {
            throw new Exception("No forex data found in the response. Verify the currency pair and date range.");
        }

        return data;
    }

    // Save the data to a CSV file
    public void SaveForexDataToCsv(JObject data, string filePath)
    {
        if (data == null || !data.HasValues)
        {
            throw new ArgumentException("Data is null or empty.", nameof(data));
        }

        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Date,Open,High,Low,Close,Volume");

            foreach (var result in data["results"])
            {
                var date = DateTimeOffset.FromUnixTimeMilliseconds((long)result["t"]).DateTime.ToString("yyyy-MM-dd");
                var open = result["o"]?.ToString() ?? "N/A";
                var high = result["h"]?.ToString() ?? "N/A";
                var low = result["l"]?.ToString() ?? "N/A";
                var close = result["c"]?.ToString() ?? "N/A";
                var volume = result["v"]?.ToString() ?? "N/A";

                writer.WriteLine($"{date},{open},{high},{low},{close},{volume}");
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
