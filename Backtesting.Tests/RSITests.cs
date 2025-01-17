using System.Collections.Generic;
using Backtesting.Indicators;
using Xunit;

namespace Backtesting.Tests
{
    public class RSITests
    {
        [Fact]
        public void Calculate_Should_Return_Correct_RSI_Values()
        {
            // Arrange
            List<double> data = new List<double> { 10, 12, 14, 16, 18, 20, 22, 24, 26, 28 };
            RSI rsi = new RSI(3);

            // Act
            rsi.Calculate(data);

            // Assert
            Assert.Equal(new List<double> { 100, 100, 100, 100, 100, 100, 100 }, rsi.RSIValues);
        }
    }
}
