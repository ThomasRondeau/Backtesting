using System;
using System.IO;
using System.Linq;
using StrategyTradeSoft;


class Program
{
    static void Main(string[] args)
    {
        
        string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
        string tempFolderPath = @"C:\Users\marco\Documents\Project_Commando_C#\Backtesting Tradesoft\Backtesting\DataLoaderSol\temp";


        try
        {
            
            if (!Directory.Exists(tempFolderPath))
            {
                Console.WriteLine($"The directory '{tempFolderPath}' does not exist.");
                return;
            }

            var files = Directory.GetFiles(tempFolderPath, "*.csv").ToList();

            if (files.Count == 0)
            {
                Console.WriteLine("No CSV files found in the temp folder.");
                return;
            }

            
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

            
            string selectedFile = files[choice - 1];
            Console.WriteLine($"You selected: {Path.GetFileName(selectedFile)}");

            
            var ticks = DataLoader.LoadTicks(selectedFile);

            
            Strategy myStrategy = new MovingAverageCrossover(5, 20);

            
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

