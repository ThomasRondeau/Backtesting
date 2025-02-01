using IndicatorsApp.Indicators;
using OrderExecutor.Classes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StrategyTradeSoft
{
    public class StrategyExecutor : IStrategyExecutor
    {
        public List<Product> Products { get; private set; }

        public List<Indicators> IndicatorsList { get; private set; }

        public List<Tick> Data { get; private set; }

        private Dictionary<Indicators, List<Product>> IndicatorToProductsMap;

        public OrderService orderService { get; init; }

        private List<string> LogMessages;

        public StrategyExecutor(OrderService orderService)
        {
            Products = new List<Product>();
            IndicatorsList = new List<Indicators>();
            Data = new List<Tick>();
            LogMessages = new List<string>();
            IndicatorToProductsMap = new Dictionary<Indicators, List<Product>>();

            this.orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        public void RunPortfolio()
        {
            foreach (var tick in Data)
            {
                foreach (var indicator in IndicatorsList)
                {
                    indicator.Calculate(new List<double> { tick.Price });
                }

                foreach (var indicator in IndicatorsList)
                {
                    if (IndicatorToProductsMap.TryGetValue(indicator, out var relatedProducts))
                    {
                        foreach (var product in relatedProducts)
                        {
                            foreach (Condition condition in product.Conditions.Where(c => c.Indicator == indicator))
                            {
                                if (condition.Indicator.LastValue > condition.Value && condition.Type == OrderType.Sell)
                                {
                                    //orderService.Sell(product, tick.Price);
                                }
                                else if (condition.Indicator.LastValue < condition.Value && condition.Type == OrderType.Buy)
                                {
                                    //orderService.Buy(product, tick.Price);
                                }
                            }
                        }
                    }
                }

                Log($"Processed tick at {tick.Time} with price {tick.Price}");
            }
        }



        public void AddProduct(Product product)
        {
            Products.Add(product);
            foreach (var condition in product.Conditions)
            {
                if (!IndicatorsList.Contains(condition.Indicator))
                    IndicatorsList.Add(condition.Indicator);

                if (!IndicatorToProductsMap.ContainsKey(condition.Indicator))
                {
                    IndicatorToProductsMap[condition.Indicator] = new List<Product>();
                }
                IndicatorToProductsMap[condition.Indicator].Add(product);
            }
        }

        private void Log(string message)
        {
            LogMessages.Add($"{DateTime.Now}: {message}");
            // ajouter quelque chose pour transferer le pourcentage de progression dans la page loadingScreen
            Console.WriteLine(message);
        }

        public void SaveLogs(string filePath)
        {
            System.IO.File.WriteAllLines(filePath, LogMessages);
        }
    }
}