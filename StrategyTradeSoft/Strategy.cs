using System;

namespace StrategyTradeSoft.Classes
{
	public abstract class Strategy
	{
		public int Id { get; private set; }
		protected static int IdCounter = 0;

		protected List<string> LogMessages = new List<string>();

		public Strategy()
		{
			Id = IdCounter++;
		}

		public abstract void Next(Tick tick);

		
		protected void Log(string message)
		{
			LogMessages.Add($"{DateTime.Now}: {message}");
			Console.WriteLine(message);
		}

		
		public void SaveLogs(string filePath)
		{
			System.IO.File.WriteAllLines(filePath, LogMessages);
		}
	}
}
