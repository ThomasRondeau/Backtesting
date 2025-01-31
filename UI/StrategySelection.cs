using IndicatorsApp.Indicators;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Reflection;

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
            productBlock.Size = new Size(400, 220);

            Label nameLabel = new Label() { Text = "Investment name", Location = new Point(10, 20) };
            TextBox nameTextBox = new TextBox() { Location = new Point(120, 20), Width = 200 };

            Label productLabel = new Label() { Text = "Product", Location = new Point(10, 140) };
            ComboBox productComboBox = new ComboBox() { Location = new Point(120, 140), Width = 200 };

            productComboBox.Items.AddRange(new object[]{ "Currency", "Options", "Futures" });

            Label quantityLabel = new Label() { Text = "Notional", Location = new Point(10, 80) };
            NumericUpDown quantityNumeric = new NumericUpDown() { Location = new Point(120, 80), Width = 200 };

            Label indicatorLabel = new Label() { Text = "Indicateur", Location = new Point(10, 140) };
            ComboBox indicatorComboBox = new ComboBox() { Location = new Point(120, 140), Width = 200 };

            indicatorComboBox.Items.AddRange([.. indicatorsName]);

            Button deleteButton = new Button() { Text = "Supprimer", Location = new Point(10, 180), Width = 200, Height = 40 };
            deleteButton.Click += (s, e) => flowLayoutPanel1.Controls.Remove(productBlock);

            productBlock.Controls.Add(nameLabel);
            productBlock.Controls.Add(nameTextBox);
            productBlock.Controls.Add(quantityLabel);
            productBlock.Controls.Add(quantityNumeric);
            productBlock.Controls.Add(productLabel);
            productBlock.Controls.Add(productComboBox);
            productBlock.Controls.Add(indicatorLabel);
            productBlock.Controls.Add(indicatorComboBox);
            productBlock.Controls.Add(deleteButton);

            flowLayoutPanel1.Controls.Add(productBlock);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach(GroupBox productBlock in flowLayoutPanel1.Controls)
            {
                TextBox nameTextBox = (TextBox)productBlock.Controls[1];
                NumericUpDown quantityNumeric = (NumericUpDown)productBlock.Controls[3];
                ComboBox typeComboBox = (ComboBox)productBlock.Controls[5];
                JObject data = new JObject();
                data["name"] = nameTextBox.Text;
                data["quantity"] = quantityNumeric.Value;
                data["type"] = typeComboBox.SelectedItem.ToString();
                data["indicators"] = new JArray();
                for (var j = 0; j < indicators.Count; j++)
                {
                    GroupBox indicatorBlock = (GroupBox)productBlock.Controls[j + 6];
                    NumericUpDown indicatorValue = (NumericUpDown)indicatorBlock.Controls[1];
                    JObject indicatorData = new JObject();
                    indicatorData["name"] = indicators[j].Name;
                    indicatorData["value"] = indicatorValue.Value;
                    data["indicators"].Add(indicatorData);
                }
                data["data"] = this.data.data;
                // Send data to the server
            }   
            LoadingScreen loadPage = new LoadingScreen(Navigator);
            Navigator.GoTo(loadPage, data);
        }
    }
}
