namespace TestPolygon;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using RestSharp;
using Newtonsoft.Json.Linq;

public class ForexDataLoader
{
    private string _apiKey;
    private RestClient _client;
    private string _xmlFilePath;

    public bool Verbose { get; set; } // Enables or disables verbose output

    // Initialize the data loader without API key
    public ForexDataLoader(bool verbose = false, string xmlFilePath = "ForexData.xml")
    {
        _client = new RestClient("https://api.polygon.io");
        Verbose = verbose;
        _xmlFilePath = xmlFilePath;
        _apiKey = "";

        EnsureXmlFileExists();
    }

    // Ensure the XML file exists, create and format it if necessary
    private void EnsureXmlFileExists()
    {
        if (!File.Exists(_xmlFilePath))
        {
            if (Verbose)
            {
                Console.WriteLine($"[INFO] XML file not found. Creating new file: {_xmlFilePath}");
            }

            // Create a new XML file with a root element and ForexEntries table
            var document = new XDocument(
                new XElement("ForexData",
                    new XElement("ForexEntries", 
                        new XElement("Columns",
                            new XElement("From", "From Symbol"),
                            new XElement("To", "To Symbol"),
                            new XElement("Date", "Date"),
                            new XElement("Open", "Open"),
                            new XElement("High", "High"),
                            new XElement("Low", "Low"),
                            new XElement("Close", "Close"),
                            new XElement("Volume", "Volume")
                        )
                    )
                )
            );
            document.Save(_xmlFilePath);

            if (Verbose)
            {
                Console.WriteLine("[INFO] XML file created and formatted.");
            }
        }
        else if (Verbose)
        {
            Console.WriteLine("[INFO] XML file exists and is ready to use.");
        }
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
            Console.WriteLine("[INFO] API key initialized successfully.");
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
            Console.WriteLine("[INFO] API key initialized from environment variables.");
        }
    }

    // Fetch forex data for a specific pair over a given period
    public async Task<JObject> FetchForexDataFromApi(string fromSym, string toSym, DateTime startDate, DateTime endDate)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new InvalidOperationException("API key is not initialized. Call Initialize() with a valid API key.");
        } 
        var request = new RestRequest($"/v2/aggs/ticker/C:{fromSym}{toSym}/range/1/day/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}", Method.Get);

        request.AddParameter("apiKey", _apiKey);

        if (Verbose)
        {
            Console.WriteLine($"[INFO] Fetching forex data for: {fromSym}/{toSym}, Start Date: {startDate:yyyy-MM-dd}, End Date: {endDate:yyyy-MM-dd}");
        }

        var response = await _client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            throw new Exception($"[ERROR] Error fetching data from Polygon API: {response.Content}");
        }

        var data = JObject.Parse(response.Content);

        if (!data.ContainsKey("results"))
        {
            throw new Exception("[ERROR] No forex data found in the response. Verify the currency pair and date range.");
        }

        if (Verbose)
        {
            Console.WriteLine("[INFO] Data fetched successfully. Displaying a snippet:");
            var firstResult = data["results"]?.FirstOrDefault();
            if (firstResult != null)
            {
                Console.WriteLine($"    - First Entry: {firstResult}");
                Console.WriteLine($"    - Total Entries: {data["results"]?.Count()}");
            }
            else
            {
                Console.WriteLine("    - No data available in results.");
            }
        }

        return data;
    }

    // Save the data to the XML file
    public void SaveForexDataToXml(JObject data, string fromSym, string toSym)
    {
        if (data == null || !data.HasValues)
        {
            throw new ArgumentException("Data is null or empty.", nameof(data));
        }

        var document = XDocument.Load(_xmlFilePath);
        var forexEntries = document.Root?.Element("ForexEntries") ?? throw new Exception("Invalid XML structure.");

        foreach (var result in data["results"])
        {
            var date = DateTimeOffset.FromUnixTimeMilliseconds((long)result["t"]).DateTime.ToString("yyyy-MM-dd");
            var open = result["o"]?.ToString() ?? "N/A";
            var high = result["h"]?.ToString() ?? "N/A";
            var low = result["l"]?.ToString() ?? "N/A";
            var close = result["c"]?.ToString() ?? "N/A";
            var volume = result["v"]?.ToString() ?? "N/A";

            // Check for duplicates
            var existingEntry = forexEntries.Elements("ForexEntry")
                .FirstOrDefault(e =>
                    e.Element("From")?.Value == fromSym &&
                    e.Element("To")?.Value == toSym &&
                    e.Element("Date")?.Value == date);

            if (existingEntry != null)
            {
                if (Verbose)
                {
                    Console.WriteLine($"[INFO] Duplicate entry found for {fromSym} -> {toSym} on {date}. Skipping.");
                }
                continue;
            }

            // Add new entry
            var entry = new XElement("ForexEntry",
                new XElement("From", fromSym),
                new XElement("To", toSym),
                new XElement("Date", date),
                new XElement("Open", open),
                new XElement("High", high),
                new XElement("Low", low),
                new XElement("Close", close),
                new XElement("Volume", volume)
            );

            forexEntries.Add(entry);
        }

        document.Save(_xmlFilePath);

        if (Verbose)
        {
            Console.WriteLine("[INFO] Data saved to XML file successfully.");
        }
    }

    public JObject FetchForexDataFromXml(string fromSymbol, string toSymbol, DateTime startDate, DateTime endDate)
    {
        var document = XDocument.Load(_xmlFilePath);
        var forexEntries = document.Root?.Element("ForexEntries") ?? throw new Exception("Invalid XML structure.");

        var dataEntries = forexEntries.Elements("ForexEntry")
            .Where(e =>
                e.Element("From")?.Value == fromSymbol &&
                e.Element("To")?.Value == toSymbol &&
                DateTime.Parse(e.Element("Date")?.Value ?? "") >= startDate &&
                DateTime.Parse(e.Element("Date")?.Value ?? "") <= endDate)
            .Select(e => new
            {
                Date = DateTime.Parse(e.Element("Date")?.Value ?? ""),
                Open = e.Element("Open")?.Value,
                High = e.Element("High")?.Value,
                Low = e.Element("Low")?.Value,
                Close = e.Element("Close")?.Value,
                Volume = e.Element("Volume")?.Value
            })
            .OrderBy(e => e.Date)
            .ToList();

        // Check for missing data and validate criteria
        int requiredEntries = (endDate - startDate).Days / 2;
        if (dataEntries.Count < requiredEntries)
        {
            if (Verbose)
            {
                Console.WriteLine("[INFO] Not enough data entries found in XML.");
            }
            return null;
        }

        bool hasStartDateClose = dataEntries.Any(e => (e.Date - startDate).Days <= 3);
        bool hasEndDateClose = dataEntries.Any(e => (endDate - e.Date).Days <= 3);

        if (!hasStartDateClose || !hasEndDateClose)
        {
            if (Verbose)
            {
                Console.WriteLine("[INFO] No data entry close to start or end date in XML.");
            }
            return null;
        }

        // Convert to JObject
        var results = new JArray(dataEntries.Select(e => new JObject
        {
            ["Date"] = e.Date.ToString("yyyy-MM-dd"),
            ["Open"] = e.Open,
            ["High"] = e.High,
            ["Low"] = e.Low,
            ["Close"] = e.Close,
            ["Volume"] = e.Volume
        }));

        return new JObject { ["results"] = results };
    }

    public async Task<JObject> FetchForexData(string fromSymbol, string toSymbol, DateTime startDate, DateTime endDate)
    {
        // Try to fetch data from XML
        var data = FetchForexDataFromXml(fromSymbol, toSymbol, startDate, endDate);
        if (data != null)
        {
            if (Verbose)
            {
                Console.WriteLine("[INFO] Data fetched from XML.");
            }
            return data;
        }

        // Fallback to API call
        if (Verbose)
        {
            Console.WriteLine("[INFO] Fetching data from API due to insufficient or missing data in XML.");
        }

        var apiData = await FetchForexDataFromApi(fromSymbol, toSymbol, startDate, endDate);

        // Save data to XML
        SaveForexDataToXml(apiData, fromSymbol, toSymbol);

        return apiData;
    }

    public void ResetXML()
    {
        var document = new XDocument(
            new XElement("ForexData",
                new XElement("ForexEntries",
                    new XElement("Columns",
                        new XElement("From", "From Symbol"),
                        new XElement("To", "To Symbol"),
                        new XElement("Date", "Date"),
                        new XElement("Open", "Open"),
                        new XElement("High", "High"),
                        new XElement("Low", "Low"),
                        new XElement("Close", "Close"),
                        new XElement("Volume", "Volume")
                    )
                )
            )
        );

        document.Save(_xmlFilePath);

        if (Verbose)
        {
            Console.WriteLine("[INFO] XML database has been reset to its initial state.");
        }
    }
}
