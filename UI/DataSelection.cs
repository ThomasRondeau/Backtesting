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
        public DataSelection(INavigator navigator) : base(navigator)
        {
            InitializeComponent();
        }

        public List<string> getData()
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StrategySelection stratPage = new StrategySelection(Navigator);
            Navigator.GoTo(stratPage);
        }
    }
}
