using Microsoft.Extensions.DependencyInjection;
using ScottPlot.Statistics;

namespace UI
{
    public class MainForm : Form, INavigator
    {
        private readonly Panel hostPanel;
        public Page? Current { get; private set; }

        public MainForm()
        {
            Size = new Size(1920, 1080);
            hostPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(hostPanel);

            Welcome welcomePage = new Welcome(this);//Program.services.GetRequiredService<Welcome>();
            GoTo(welcomePage);
        }

        public void GoTo(Page newPage, object? loadData = null)
        {
            Current?.BeforeClose();

            hostPanel.Controls.Clear();
            newPage.Dock = DockStyle.Fill;
            hostPanel.Controls.Add(newPage);

            newPage.BeforeLoad(loadData);
            Current = newPage;
        }
    }
}