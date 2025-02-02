using DataLoader;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace UI
{
    public partial class DataSelection : Page
    {
        private readonly IDataService _dataService;
        public DataSelection(INavigator navigator, IDataService dataService) : base(navigator)
        {
            InitializeComponent();
            _dataService = dataService;
        }

        public async Task<JObject> getDataAsync(string fromCurrency, string toCurrency, DateTime fromDate, DateTime toDate)
        {
            return await _dataService.FetchForexDataAsync(fromCurrency, toCurrency, fromDate, toDate);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Recup parameters
            var data = await getDataAsync();
            Console.WriteLine(data);
            StrategySelection stratPage = Program.services.GetRequiredService<StrategySelection>();
            Navigator.GoTo(stratPage, new CacheData(data: data));
        }
    }
}
