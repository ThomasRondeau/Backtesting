using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataLoader;
using StrategyTradeSoft;
using OrderExecutor.Classes;

namespace UI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var host = CreateHostBuilder().Build();
            var services = host.Services;

            ApplicationConfiguration.Initialize();
            var mainForm = services.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<INavigator, MainForm>();
                    services.AddSingleton<IDataService, DataService>();
                    services.AddSingleton<IStrategyExecutor, StrategyExecutor>();
                    services.AddSingleton<IOrderService, OrderService>();

                    services.AddTransient<MainForm>();

                    services.AddTransient<DataSelection>();
                    services.AddTransient<StrategySelection>();
                    services.AddTransient<LoadingScreen>();
                    services.AddTransient<Output>();
                });
        }
    }
}
