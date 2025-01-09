namespace TestAlpha;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;
using dotenv.net;

public class DataLoader
{
    private readonly string _apiKey;
    private readonly RestClient _client;
    private readonly string _tempFolderPath;

    public DataLoader()
    {
        // Load environment variables
        DotEnv.Load();

        // Get the API key
        _apiKey = Environment.GetEnvironmentVariable("ALPHA_VANTAGE_API_KEY");
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new Exception("API key is missing. Ensure it's set in the .env file.");
        }

        // Initialize the RestClient
        _client = new RestClient("https://www.alphavantage.co");

        // Initialize the temp folder path inside the project directory
        _tempFolderPath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName ?? ".", "temp");

        // Create the temp folder if it does not exist
        if (!Directory.Exists(_tempFolderPath))
        {
            Directory.CreateDirectory(_tempFolderPath);
        }
    }

    public async Task<JObject> LoadDataAsync(string fromSymbol, string toSymbol, DateTime startDate, DateTime endDate, string function = "FX_DAILY", string outputSize = "full")
    {
        // Build the API request
        var request = new RestRequest("/query", Method.Get);
        request.AddParameter("function", function);
        request.AddParameter("from_symbol", fromSymbol);
        request.AddParameter("to_symbol", toSymbol);
        request.AddParameter("apikey", _apiKey);
        request.AddParameter("outputsize", outputSize);

        // Execute the request
        var response = await _client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            // Parse the response content into a JObject
            var data = JObject.Parse(response.Content);

            if (data.ContainsKey("Time Series FX (Daily)"))
            {
                var timeSeriesData = data["Time Series FX (Daily)"] as JObject;

                // Filter the time series data based on the date range
                var filteredData = new JObject();

                foreach (var property in timeSeriesData.Properties())
                {
                    DateTime entryDate = DateTime.Parse(property.Name);
                    if (entryDate >= startDate && entryDate <= endDate)
                    {
                        filteredData.Add(property.Name, property.Value);
                    }
                }

                // Print the first 5 rows of the filtered data
                Console.WriteLine("First 5 rows of data:");
                //int rowCount = 0;
                foreach (var property in filteredData.Properties().Take(5))
                {
                    Console.WriteLine($"Date: {property.Name}, Data: {property.Value}");
                    //rowCount++;
                }

                /*if (rowCount == 0)
                {
                    Console.WriteLine("No data available for the specified date range.");
                }*/

                return filteredData;
            }
            else
            {
                Console.WriteLine(data);
                throw new Exception("Error: No 'Time Series FX (Daily)' data found in the response.");
            }
        }
        else
        {
            throw new Exception($"Error fetching data from Alpha Vantage: {response.StatusDescription}");
        }
    }

    public void SaveDataToCsv(JObject timeSeriesData, string fromSymbol, string toSymbol)
    {
        // Build the file name dynamically
        string fileName = $"{fromSymbol}-{toSymbol}.csv";
        string filePath = Path.Combine(_tempFolderPath, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Date,Open,High,Low,Close");

            foreach (var property in timeSeriesData.Properties())
            {
                string date = property.Name; // Get the date key
                var details = property.Value as JObject;
                string open = details?["1. open"]?.ToString() ?? "N/A";
                string high = details?["2. high"]?.ToString() ?? "N/A";
                string low = details?["3. low"]?.ToString() ?? "N/A";
                string close = details?["4. close"]?.ToString() ?? "N/A";

                writer.WriteLine($"{date},{open},{high},{low},{close}");
            }
        }

        Console.WriteLine($"Data successfully saved to: {filePath}");
    }

    public async Task LoadFromCsvAsync(string csvPath, DateTime startDate, DateTime endDate)
    {
        if (!File.Exists(csvPath))
        {
            Console.WriteLine($"Error: The input CSV file was not found. Current path: {Directory.GetCurrentDirectory()}");
            throw new FileNotFoundException("The specified CSV file does not exist.", csvPath);
        }

        try
        {
            using (var reader = new StreamReader(csvPath))
            {
                // Read the header line
                var headerLine = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(headerLine))
                {
                    throw new Exception("The CSV file is empty.");
                }

                var headers = headerLine.Split(',');
                int fromSymbolIndex = Array.IndexOf(headers, "fromSymbol");
                int toSymbolIndex = Array.IndexOf(headers, "toSymbol");

                if (fromSymbolIndex == -1 || toSymbolIndex == -1)
                {
                    throw new Exception("The CSV file must contain 'fromSymbol' and 'toSymbol' columns.");
                }

                // Process each line in the CSV
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var columns = line.Split(',');

                    if (columns.Length <= Math.Max(fromSymbolIndex, toSymbolIndex))
                    {
                        Console.WriteLine("Skipping invalid line in CSV: " + line);
                        continue;
                    }

                    string fromSymbol = columns[fromSymbolIndex].Trim();
                    string toSymbol = columns[toSymbolIndex].Trim();

                    if (string.IsNullOrWhiteSpace(fromSymbol) || string.IsNullOrWhiteSpace(toSymbol))
                    {
                        Console.WriteLine("Skipping line with missing symbols: " + line);
                        continue;
                    }

                    Console.WriteLine($"Processing {fromSymbol}/{toSymbol}");

                    try
                    {
                        // Download data and save it
                        var data = await LoadDataAsync(fromSymbol, toSymbol, startDate, endDate);
                        SaveDataToCsv(data, fromSymbol, toSymbol);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing {fromSymbol}/{toSymbol}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading the CSV file: {ex.Message}");
        }
    }
    
    public static void TestEnv()
    {
        Console.WriteLine($"Répertoire courant : {Directory.GetCurrentDirectory()}");

        try
        {
            // Charge les variables d'environnement
            DotEnv.Load();

            // Vérifie si la clé API est bien chargée
            string apiKey = Environment.GetEnvironmentVariable("ALPHA_VANTAGE_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("Erreur : La clé API n'est pas chargée. Vérifiez le fichier .env.");
            }
            else
            {
                Console.WriteLine($"Clé API chargée avec succès : {apiKey}");
            }

            // Vérifie le chemin courant pour voir où il cherche le fichier .env
            Console.WriteLine($"Répertoire courant : {Directory.GetCurrentDirectory()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du test du fichier .env : {ex.Message}");
        }
    }

}
