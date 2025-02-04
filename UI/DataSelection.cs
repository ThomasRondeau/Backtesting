using DataLoader;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace UI
{
    public partial class DataSelection : Page
    {
        private readonly IDataService _dataService;
        public DataSelection(INavigator navigator) : base(navigator)
        {
            InitializeComponent();
            _dataService = Program.services.GetRequiredService<IDataService>();
        }

        public async Task<JObject> getDataAsync(string fromCurrency, string toCurrency, DateTime fromDate, DateTime toDate)
        {
            return await _dataService.FetchForexDataAsync(fromCurrency, toCurrency, fromDate, toDate);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string[] currencies = comboBox1.SelectedItem.ToString().Split("-");
            var data = await getDataAsync(currencies[0], currencies[1], dateTimePicker1.Value, dateTimePicker2.Value);
            Console.WriteLine(data);
            StrategySelection stratPage = Program.services.GetRequiredService<StrategySelection>();
            Navigator.GoTo(stratPage, new CacheData(data: data));
        }
    }
}