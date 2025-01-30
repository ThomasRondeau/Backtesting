using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataLoader;

namespace UI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
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
                    services.AddSingleton<MainForm>();
                    services.AddSingleton<IDataService, DataLoader.>();
                });
        }
    }
}
