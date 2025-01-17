using System;
using System.Collections.Generic;

class Program
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
    }
}
