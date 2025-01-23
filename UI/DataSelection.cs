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

        public List<string> getFiles()
        {
            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
            string tempFolderPath = Path.Combine(solutionDirectory, "DataLoaderSol", "temp");
            List<string> files = Directory.GetFiles(tempFolderPath, "*.csv").ToList();
            return files;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StrategySelection stratPage = new StrategySelection(Navigator);
            Navigator.GoTo(stratPage);
        }
    }
}
