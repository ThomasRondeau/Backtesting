using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TestAlpha
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Define the path to the input CSV file
                string csvPath = "../../../input.csv";

                // Ensure the CSV file exists
                if (!File.Exists(csvPath))
                {
                    Console.WriteLine($"Error: The input CSV file was not found. Current path: {Directory.GetCurrentDirectory()}");
                    throw new FileNotFoundException("The specified CSV file does not exist.", csvPath);
                }

                // Prompt the user for start and end dates
                Console.Write("Enter the start date (yyyy-MM-dd): ");
                if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
                {
                    Console.WriteLine("Invalid start date. Please use the format yyyy-MM-dd.");
                    return;
                }

                Console.Write("Enter the end date (yyyy-MM-dd): ");
                if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
                {
                    Console.WriteLine("Invalid end date. Please use the format yyyy-MM-dd.");
                    return;
                }

                // Create an instance of the DataLoader
                var dataLoader = new DataLoader();

                // Process the CSV file
                Console.WriteLine("Processing data from the CSV file...");
                await dataLoader.LoadFromCsvAsync(csvPath, startDate, endDate);

                Console.WriteLine("Data loading and saving completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}