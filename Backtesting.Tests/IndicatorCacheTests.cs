using System;
using Xunit;
using IndicatorsApp.Indicators;

namespace Backtesting.Tests
{
    public class IndicatorCacheTests
    {
        [Fact]
        public void Add_Should_Add_Values_To_Cache()
        {
            // Arrange
            IndicatorCache<double> cache = new IndicatorCache<double>(3);

            // Act
            cache.Add(1);
            cache.Add(2);
            cache.Add(3);

            // Assert
            Assert.Equal(new double[] { 1, 2, 3 }, cache.GetAll());
        }

        [Fact]
        public void Add_Should_Remove_Oldest_Value_When_Cache_Is_Full()
        {
            // Arrange
            IndicatorCache<double> cache = new IndicatorCache<double>(3);

            // Act
            cache.Add(1);
            cache.Add(2);
            cache.Add(3);
            cache.Add(4);

            // Assert
            Assert.Equal(new double[] { 2, 3, 4 }, cache.GetAll());
        }

        [Fact]
        public void Clear_Should_Empty_The_Cache()
        {
            // Arrange
            IndicatorCache<double> cache = new IndicatorCache<double>(3);
            cache.Add(1);
            cache.Add(2);
            cache.Add(3);

            // Act
            cache.Clear();

            // Assert
            Assert.Empty(cache.GetAll());
        }
    }
}
