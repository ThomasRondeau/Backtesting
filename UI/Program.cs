using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataLoader;
using StrategyTradeSoft;
using OrderExecutor.Classes;

namespace UI
{
    public static class Program
    {
        public static IServiceProvider services;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            ApplicationConfiguration.Initialize();

            var host = CreateHostBuilder().Build();
            services = host.Services;

            var mainForm = services.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<MainForm>();
                    services.AddSingleton<INavigator>(sp => sp.GetRequiredService<MainForm>());

                    // Services
                    services.AddSingleton<IDataService, DataService>();
                    services.AddSingleton<IStrategyExecutor, StrategyExecutor>();
                    services.AddSingleton<IOrderService, OrderService>();

                    // Pages
                    services.AddTransient<Welcome>();
                    services.AddTransient<DataSelection>();
                    services.AddTransient<StrategySelection>();
                    services.AddTransient<LoadingScreen>();
                    services.AddTransient<Output>();
                });
        }
    }
}
