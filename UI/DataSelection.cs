using DataLoader;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            return _dataService.FetchForexDataAsync("USD", "JPY", DateTime.Now.AddDays(-7), DateTime.Now).Result; // Modif les inputs
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            StrategySelection stratPage = new StrategySelection(Navigator);
            Navigator.GoTo(stratPage, new CacheData(data: getData()));
        }
    }
}
