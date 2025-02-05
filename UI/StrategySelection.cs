using IndicatorsApp.Indicators;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OrderExecutor.Classes;
using StrategyTradeSoft;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace UI
{
    public partial class StrategySelection : Page
    {
        List<Indicators> indicators = [];
        List<string> indicatorsName = [];

        private CacheData data;

        public override void BeforeLoad(object? loadData)
        {
            if (loadData is CacheData donnees)
            {
                data = donnees;
            }
        }

        public StrategySelection(INavigator navigator) : base(navigator)
        {
            InitializeComponent();

            foreach (Type type in
                Assembly.GetAssembly(typeof(Indicators)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Indicators))))
            {
                indicatorsName.Add(type.Name);
            }
            indicators.Sort();
        }


        private void button1_Click(object sender, System.EventArgs e)
        {
            AddProductBlock();
        }

        private void AddProductBlock()
        {
            GroupBox productBlock = new GroupBox();
            productBlock.Text = $"Product {flowLayoutPanel1.Controls.Count + 1}";
            productBlock.Size = new Size(400, 400);

            Label nameLabel = new Label() { Text = "Name", Location = new Point(10, 20) };
            TextBox nameTextBox = new TextBox() { Location = new Point(120, 20), Width = 200 };

            Label quantityLabel = new Label() { Text = "Notional", Location = new Point(10, 60) };
            NumericUpDown quantityNumeric = new NumericUpDown() { Location = new Point(120, 60), Width = 200 };

            Label productLabel = new Label() { Text = "Product", Location = new Point(10, 100) };
            ComboBox productComboBox = new ComboBox() { Location = new Point(120, 100), Width = 200 };
            productComboBox.Items.AddRange(new string[] { "Currency", "Options", "Futures" });

            Label indicatorLabel = new Label() { Text = "Indicateur", Location = new Point(10, 140) };
            ComboBox indicatorComboBox = new ComboBox() { Location = new Point(120, 140), Width = 150 };
            NumericUpDown indicatorPeriodNumeric = new NumericUpDown() { Location = new Point(290, 140), Width = 50 };
            indicatorComboBox.Items.AddRange(indicatorsName.ToArray());

            Label orderType = new Label() { Text = "Buy or sell", Location = new Point(10, 180) };
            ComboBox orderTypeComboBox = new ComboBox() { Location = new Point(120, 180), Width = 150 };
            orderTypeComboBox.Items.AddRange(new string[] { "Buy", "Sell" });

            Label limit = new Label() { Text = "Limit", Location = new Point(10, 220) };
            NumericUpDown limitValue = new NumericUpDown() { Location = new Point(120, 220), Width = 150 };
            limitValue.DecimalPlaces = 2; 
            limitValue.Increment = 0.01M; 

            Button deleteButton = new Button() { Text = "Supprimer", Location = new Point(120, 260), Width = 200, Height = 40 };
            deleteButton.Click += (s, e) => flowLayoutPanel1.Controls.Remove(productBlock);

            productBlock.Controls.Add(nameLabel);
            productBlock.Controls.Add(nameTextBox);
            productBlock.Controls.Add(quantityLabel);
            productBlock.Controls.Add(quantityNumeric);
            productBlock.Controls.Add(productLabel);
            productBlock.Controls.Add(productComboBox);
            productBlock.Controls.Add(indicatorLabel);
            productBlock.Controls.Add(indicatorComboBox);
            productBlock.Controls.Add(indicatorPeriodNumeric);
            productBlock.Controls.Add(orderType);
            productBlock.Controls.Add(orderTypeComboBox);
            productBlock.Controls.Add(limit);
            productBlock.Controls.Add(limitValue);
            productBlock.Controls.Add(deleteButton);

            flowLayoutPanel1.Controls.Add(productBlock);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (GroupBox productBlock in flowLayoutPanel1.Controls)
            {
                TextBox nameTextBox = (TextBox)productBlock.Controls[1];
                NumericUpDown quantityNumeric = (NumericUpDown)productBlock.Controls[3];
                ComboBox typeComboBox = (ComboBox)productBlock.Controls[5];
                ComboBox indicatorComboBox = (ComboBox)productBlock.Controls[7];
                NumericUpDown indicatorPeriodNumeric = (NumericUpDown)productBlock.Controls[8];
                ComboBox orderTypeComboBox = (ComboBox)productBlock.Controls[10];
                NumericUpDown limitValue = (NumericUpDown)productBlock.Controls[12];

                if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                    typeComboBox.SelectedItem == null ||
                    indicatorComboBox.SelectedItem == null ||
                    orderTypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Champs manquants", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string name = nameTextBox.Text;
                double notional = (double)quantityNumeric.Value;
                string type = typeComboBox.SelectedItem.ToString();
                string indicator = indicatorComboBox.SelectedItem.ToString();
                OrderType orderType = orderTypeComboBox.SelectedItem.ToString() == "Buy" ? OrderType.Buy : OrderType.Sell;
                double limit = (double)limitValue.Value;

                Indicators instance;
                switch (indicator)
                {
                    case "ATR":
                        instance = new ATR((int)indicatorPeriodNumeric.Value);
                        break;
                    case "MovingAverage":
                        instance = new MovingAverage((int)indicatorPeriodNumeric.Value);
                        break;
                    case "OBV":
                        instance = new OBV();
                        break;
                    case "RSI":
                        instance = new RSI((int)indicatorPeriodNumeric.Value);
                        break;
                    default:
                        throw new Exception("Indicateur non pris en charge");
                }

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition(instance, orderType, limit));
                Product product = new Product(name, notional, conditions);
                data.products.Add(product);
            }
            LoadingScreen loadPage = Program.services.GetRequiredService<LoadingScreen>();
            Navigator.GoTo(loadPage, data);
        }
    }
}
