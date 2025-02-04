using Microsoft.Extensions.DependencyInjection;
using StrategyTradeSoft;

namespace UI
{
    public partial class LoadingScreen : Page
    {
        private CacheData data;
        private IStrategyExecutor _stategyService;

        public LoadingScreen(INavigator navigator) : base(navigator)
        {
            InitializeComponent();
            _stategyService = Program.services.GetRequiredService<IStrategyExecutor>();
        }


        public override void BeforeLoad(object? loadData)
        {
            if (loadData is CacheData donnees)
            {
                data = donnees;
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            RunSimulation();
            Output outputPage = Program.services.GetRequiredService<Output>();
            Navigator.GoTo(outputPage); //TODO : Passer les données de simulation à la page de sortie
        }

        private void RunSimulation()
        {
            try
            {
                _stategyService.addData(data.data);
                data.products.ForEach(product => _stategyService.AddProduct(product));
                _stategyService.RunPortfolio();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur durant la simulation: {ex.Message}");
            }
        }
    }
}