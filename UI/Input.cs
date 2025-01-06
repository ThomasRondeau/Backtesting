using System.Windows.Forms;

namespace UI
{
    public partial class Input : Form
    {
        public Input()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            AddProductBlock();
        }

        private void AddProductBlock()
        {
            GroupBox productBlock = new GroupBox();
            productBlock.Text = $"Product {flowLayoutPanel1.Controls.Count + 1}";
            productBlock.Size = new Size(400, 150);

            Label nameLabel = new Label() { Text = "Nom du produit", Location = new Point(10, 20) };
            TextBox nameTextBox = new TextBox() { Location = new Point(120, 20), Width = 200 };

            Label quantityLabel = new Label() { Text = "Quantité", Location = new Point(10, 50) };
            NumericUpDown quantityNumeric = new NumericUpDown() { Location = new Point(120, 50), Width = 200 };

            Label typeLabel = new Label() { Text = "Type de produit", Location = new Point(10, 80) };
            ComboBox typeComboBox = new ComboBox() { Location = new Point(120, 80), Width = 200 };
            typeComboBox.Items.AddRange(new string[] { "Action", "Obligation", "ETF", "Autre" });

            Button deleteButton = new Button() { Text = "Supprimer", Location = new Point(10, 110) };
            deleteButton.Click += (s, e) => flowLayoutPanel1.Controls.Remove(productBlock);

            productBlock.Controls.Add(nameLabel);
            productBlock.Controls.Add(nameTextBox);
            productBlock.Controls.Add(quantityLabel);
            productBlock.Controls.Add(quantityNumeric);
            productBlock.Controls.Add(typeLabel);
            productBlock.Controls.Add(typeComboBox);
            productBlock.Controls.Add(deleteButton);

            flowLayoutPanel1.Controls.Add(productBlock);
        }
    }
}
