using System;
using System.IO;
using System.Linq;
using StrategyTradeSoft;


class Program
{
    static void Main(string[] args)
    {
        // Move up from the executable directory to the solution directory and locate the temp folder
        string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
        string tempFolderPath = Path.Combine(solutionDirectory, "DataLoaderSol", "temp");

        try
        {
            // Verify if the directory exists
            if (!Directory.Exists(tempFolderPath))
            {
                Console.WriteLine($"The directory '{tempFolderPath}' does not exist.");
                return;
            }

            // List all CSV files in the "temp" folder
            var files = Directory.GetFiles(tempFolderPath, "*.csv").ToList();

            if (files.Count == 0)
            {
                Console.WriteLine("No CSV files found in the temp folder.");
                return;
            }

            // Display the list of files for the user to choose
            Console.WriteLine("Available currency pairs:");
            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
            }

            Console.Write("Enter the number of the currency pair to load: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > files.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            // Get the selected file
            string selectedFile = files[choice - 1];
            Console.WriteLine($"You selected: {Path.GetFileName(selectedFile)}");

            // Load tick data from the selected file
            var ticks = DataLoader.LoadTicks(selectedFile);

            // Instantiate your strategy (adjust parameters as needed)
            Strategy myStrategy = new MovingAverageCrossover(5, 20);

            // Process each tick using the strategy
            foreach (var tick in ticks)
            {
                myStrategy.Next(tick);
            }

            Console.WriteLine("Processing complete. Press Enter to exit...");
            Console.WriteLine($"Temp folder path: {tempFolderPath}");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

