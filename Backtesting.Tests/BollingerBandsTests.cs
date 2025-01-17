using System.Collections.Generic;
using Backtesting.Indicators;
using Xunit;

namespace Backtesting.Tests
{
    public class BollingerBandsTests
    {
        [Fact]
        public void Calculate_Should_Return_Correct_Bands()
        {
            // Arrange
            List<double> data = new List<double> { 10, 12, 14, 16, 18, 20, 22, 24, 26, 28 };
            BollingerBands bb = new BollingerBands(3, 2);

            // Act
            bb.Calculate(data);

            // Assert
            Assert.Equal(new List<double> { 12, 14, 16, 18, 20, 22, 24, 26 }, bb.MiddleBand);
            Assert.Equal(new List<double> { 14, 16, 18, 20, 22, 24, 26, 28 }, bb.UpperBand);
            Assert.Equal(new List<double> { 10, 12, 14, 16, 18, 20, 22, 24 }, bb.LowerBand);
        }
    }
}
