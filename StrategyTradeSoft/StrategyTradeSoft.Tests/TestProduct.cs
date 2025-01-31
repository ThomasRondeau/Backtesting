using NUnit.Framework;
using StrategyTradeSoft;
using IndicatorsApp.Indicators;
using System;
using System.Collections.Generic;

namespace StrategyTradeSoft.Tests
{
	[TestFixture]
	public class TestProduct
	{
		private Product _product;

		[SetUp]
		public void Setup()
		{
			// Sample indicators (e.g., RSI)
			var indicators = new List<Indicators>
			{
				new RSI(14),
				new MovingAverage(10)
			};

			// Initialize a product
			_product = new Product(1, "Product", 100000, "EUR-USD", indicators, DateOnly.FromDateTime(DateTime.Now.AddDays(-30)), DateOnly.FromDateTime(DateTime.Now));
		}

		[Test]
		public void Test_ProductInitialization()
		{
			Assert.IsNotNull(_product);
			Assert.AreEqual("EUR-USD", _product.Name);
			Assert.AreEqual(100000, _product.Notional);
			Assert.AreEqual(2, _product.IndicatorsList.Count);
		}

		[Test]
		public void Test_ExecuteStrategy()
		{
			var sampleData = new List<Tick>
			{
				new Tick(DateTime.Now, "EUR-USD", 1000, 1.105),
                new Tick( DateTime.Now.AddMinutes(1), "EUR-USD", 1000, 1.107 ),
                new Tick( DateTime.Now.AddMinutes(2), "EUR-USD", 1000, 1.110 ),

            };

			Assert.DoesNotThrow(() => _product.RunStrategy(sampleData));
		}
	}
}