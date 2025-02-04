using IndicatorsApp.Indicators;
using OrderExecutor.Classes;
using ScottPlot.WinForms;

namespace UI
{
    public partial class Output : Page
    {
        private OutputData data;

        public override void BeforeLoad(object? loadData)
        {
            if (loadData is OutputData donnees)
            {
                data = donnees;
            }
        }
        public Output(INavigator navigator) : base(navigator)
        {
            InitializeComponent();
            PopulateCells();

            List<double[]> Xdraws = [];
            List<double[]> Ydraws = [];

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
            TraceGraphs(formsPlot1, Xdraws, Ydraws);
        }

        public void PopulateCells()
        {
            List<Position> positions = new List<Position>();
            positions.AddRange(data.Portfolio.Positions);
            positions.AddRange(data.Portfolio.ClosedPositions);

            foreach (Position position in positions) {
                string[] ligne = new string[]
                {
                position.ProductId.ToString(),
                position.EntryPrice.ToString(),
                position.EntryTime.ToString(),
                position.ExitPrice.ToString(),
                position.ExitTime.ToString(),
                position.ProfitLoss.ToString()
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
}
