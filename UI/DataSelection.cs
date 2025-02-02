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

        public JObject getData()
        {
            return _dataService.FetchForexDataAsync("USD", "JPY", DateTime.Now.AddDays(-7), DateTime.Now).Result;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            StrategySelection stratPage = Program.services.GetRequiredService<StrategySelection>(); ;
            Navigator.GoTo(stratPage, new CacheData(data: getData()));
        }
    }
}
