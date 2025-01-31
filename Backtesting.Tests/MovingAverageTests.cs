using System.Collections.Generic;
using IndicatorsApp.Indicators;
using Xunit;

namespace Backtesting.Tests
{
    public class MovingAverageTests
    {
        [Fact]
        public void Calculate_Should_Return_Correct_Moving_Averages()
        {
            // Arrange
            List<double> data = new List<double> { 10, 12, 14, 16, 18, 20, 22, 24, 26, 28 };
            MovingAverage ma = new MovingAverage(3);

            // Act
            ma.Calculate(data);

            // Assert
            Assert.Equal(new List<double> { 12, 14, 16, 18, 20, 22, 24, 26 }, ma.Values);
        }
    }
}
