using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace DataManagement
{
    public class DataLoaderBDD
    {
        private string _apiKey;
        private RestClient _client;
        private string _xmlFilePath;

        public bool Verbose { get; set; } // Enables or disables verbose output

        public DataLoaderBDD(string xmlFilePath, bool verbose = false)
        {
            _client = new RestClient("https://www.alphavantage.co");
            _xmlFilePath = xmlFilePath;
            Verbose = verbose;

            InitializeXmlFile();
        }

        private void InitializeXmlFile()
        {
            if (!File.Exists(_xmlFilePath) || new FileInfo(_xmlFilePath).Length == 0)
            {
                var root = new XElement("Stocks");
                root.Save(_xmlFilePath);
                if (Verbose)
                {
                    Console.WriteLine($"XML file initialized at {_xmlFilePath}.");
                }
            }
        }

        public void Initialize(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));
            }

            _apiKey = apiKey;

            if (Verbose)
            {
                Console.WriteLine("API key initialized successfully.");
            }
        }
    }
}