using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace StrategyTradeSoft
{
    public class DataLoader
    {
        public static List<Tick> LoadTicks(string filePath)
        {
            var ticks = new List<Tick>();

            using (var reader = new StreamReader(filePath))
            {
                string? header = reader.ReadLine();
                if (header == null)
                    throw new Exception("CSV file is empty.");

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null) continue;

                    var columns = line.Split(',');
                    var date = DateTime.Parse(columns[0]);
                    var open = float.Parse(columns[1], CultureInfo.InvariantCulture);
                    var high = float.Parse(columns[2], CultureInfo.InvariantCulture);
                    var low = float.Parse(columns[3], CultureInfo.InvariantCulture);
                    var close = float.Parse(columns[4], CultureInfo.InvariantCulture);

                    ticks.Add(new Tick(date, "OHLC", (int)(close * 1000), close));
                }
            }

            return ticks;
        }
    }
}
