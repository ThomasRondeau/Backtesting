using IndicatorsApp.Indicators;
using OrderExecutor.Classes;
using ScottPlot.Colormaps;
using ScottPlot.WinForms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UI
{
    public partial class Output : Page
    {
        private OutputData data;

        public Output(INavigator navigator) : base(navigator)
        {
            InitializeComponent();
        }
        public override void BeforeLoad(object? loadData)
        {
            if (loadData is OutputData donnees)
            {
                data = donnees;
                
                PopulateCells();

                List<double[]> Xdraws = [];
                List<double[]> Ydraws = [];
                //Draw indicators graph
                foreach (Indicators indicator in data.indicators)
                {
                    List<double> dataX = [];
                    List<double> dataY = [];
                    for (int i = 0; i < indicator.Values.Count; i++)
                    {
                        dataX.Add(i);
                        dataY.Add(indicator.Values[i]);
                    }
                    Xdraws.Add(dataX.ToArray());
                    Ydraws.Add(dataY.ToArray());
                }

                // Draw data graph
                List<double> datax = [];
                List<double> datay = [];
                int temp =0;
                foreach (var result in data.data["results"])
                {
                    datax.Add(temp++);
                    datay.Add((double)result["c"]);
                }

                Xdraws.Add(datax.ToArray());
                Ydraws.Add(datay.ToArray());

                TraceGraphs(formsPlot1, Xdraws, Ydraws);
                label2.Text = "Analyse en cours...";
                LoadAIResponse();
            }
        }

        private async void LoadAIResponse()
        {
            try
            {
                string response = await GetAIResponse();
                // Comme nous sommes dans un contexte UI, pas besoin d'Invoke
                label2.Text = response;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'analyse AI : {ex.Message}");
                label2.Text = "Erreur lors de l'analyse";
            }
        }

        public void PopulateCells()
        {
            List<Position> positions = [.. data.Portfolio.Positions, .. data.Portfolio.ClosedPositions];

            foreach (Position position in positions)
            {
                string[] ligne = new string[]
                {
                position.ProductId.ToString(),
                position.EntryPrice.ToString(),
                position.EntryTime.ToString(),
                position.ExitPrice.ToString(),
                position.ExitTime.ToString(),
                position.ProfitLoss.Last().ToString()
                };
                dataGridView1.Rows.Add(ligne);
            }
        }

        public void TraceGraphs(FormsPlot graphique, List<double[]> donneesX, List<double[]> donneesY)
        {
            if (donneesX.Count != donneesY.Count)
            {
                MessageBox.Show("Le nombre de séries X et Y doit être identique");
                return;
            }

            graphique.Plot.Clear();

            for (int i = 0; i < donneesX.Count; i++)
            {
                if (donneesX[i].Length != donneesY[i].Length)
                {
                    MessageBox.Show($"Data lenghts dont match");
                    continue;
                }

                var courbe = graphique.Plot.Add.Scatter(
                    donneesX[i],
                    donneesY[i]
                );
            }

            // Ajouter une légende
            //graphique.Plot.Add.Legend(true, ScottPlot.Alignment.UpperRight);

            graphique.Refresh();
        }

        public async Task<string> GetAIResponse()
        {
            List<Position> positions = [.. data.Portfolio.Positions, .. data.Portfolio.ClosedPositions];

            string AIfeeding = "Voici les positions ouvertes durant le trading\n";
            foreach (Position position in positions)
            {
                string ligne = $"Position {position.ProductId} " +
                    $"Prix entrée {position.EntryPrice} " +
                    $"{position.EntryTime} " +
                    $"Prix Sortie {position.ExitPrice} " +
                    $"{position.ExitTime} " +
                    $"PNL {position.ProfitLoss.Last()}\n";
                AIfeeding += ligne;
            }

            AIfeeding += "\nVoici les données de trading\n";
            foreach (var result in data.data["results"])
            {
                AIfeeding += $"Tick at {DateTimeOffset.FromUnixTimeMilliseconds((long)result["t"]).DateTime:yyyy-MM-dd} with price {result["c"]}\n";
            }

            OllamaService ollama = new("llama3.2");
            return await ollama.GenerateResponse(
                "A partir des données que je vais te passer, analyse et dis moi comment j'aurais pu réaliser un meilleur investissement en prennant moins de risques : " +
                AIfeeding);
        }
    }
}