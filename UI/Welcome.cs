using Microsoft.Extensions.DependencyInjection;

namespace UI
{
    public partial class Welcome : Page
    {
        public Welcome(INavigator navigator) : base(navigator)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSelection dataPage = Program.services.GetRequiredService<DataSelection>();
            Navigator.GoTo(dataPage);
        }
    }
}
