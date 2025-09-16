using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using starterTemplate.ViewModels;
using System.Windows;

namespace starterTemplate
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Core services
            services.AddSingleton<ILogger, FileLogger>();
            
            // ViewModels
            services.AddTransient<MainWindowViewModel>();
            
            // Views
            services.AddTransient<MainWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            
            var logger = _host.Services.GetRequiredService<ILogger>();
            logger.Initialize();
            logger.LogInfo("Application starting.");

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            var logger = _host.Services.GetService<ILogger>();
            logger?.LogInfo("Application exiting.");
            
            await _host.StopAsync();
            _host.Dispose();
            
            base.OnExit(e);
        }
    }
}