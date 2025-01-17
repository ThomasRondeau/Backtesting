using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using Backtesting.Indicators;

namespace Backtesting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<double> data = new List<double> { 10, 12, 14, 16, 18, 20, 22, 24, 26, 28 };

            MovingAverage ma = new MovingAverage(3);
            ma.Calculate(data);
            ma.Display();

            RSI rsi = new RSI(3);
            rsi.Calculate(data);
            rsi.Display();

            MACD macd = new MACD(3, 6, 2);
            macd.Calculate(data);
            macd.Display();

            BollingerBands bb = new BollingerBands(20, 2);
            bb.Calculate(data);
            bb.Display();

            StochasticOscillator so = new StochasticOscillator(14);
            so.Calculate(data);
            so.Display();

            ATR atr = new ATR(14);
            atr.Calculate(data);
            atr.Display();

            OBV obv = new OBV();
            obv.Calculate(data);
            obv.Display();

            IchimokuCloud ichimoku = new IchimokuCloud(9, 26, 52, 26);
            ichimoku.Calculate(data);
            ichimoku.Display();
        }
    }
}
