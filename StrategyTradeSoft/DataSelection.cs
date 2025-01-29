using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StrategyTradeSoft
{
    public class DataSelection
    {
        private string tempFolderPath;

        public DataSelection()
        {
            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
            tempFolderPath = Path.Combine(solutionDirectory, "DataLoaderSol", "temp");
        }

        public List<string> GetAvailablePairs()
        {
            if (!Directory.Exists(tempFolderPath))
                return new List<string>();

            return Directory.GetFiles(tempFolderPath, "*.csv")
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }

        public List<Tick> LoadData(string selectedPair)
        {
            string filePath = Path.Combine(tempFolderPath, selectedPair + ".csv");
            List<Tick> ticks = new List<Tick>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath).Skip(1); // Skip header
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 2 && DateTime.TryParse(parts[0], out DateTime time) && double.TryParse(parts[1], out double price))
                    {
                        ticks.Add(new Tick(time, "Unknown", 0, (float)price)); // Updated constructor
                    }
                }
            }

            return ticks;
        }
    }
}
