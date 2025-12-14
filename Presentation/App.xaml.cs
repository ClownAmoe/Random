// Файл: App.xaml.cs (Спрощена версія без ShellViewModel)

using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Presentation.Services;
using Presentation.ViewModels;
using System;

namespace Presentation
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            // ... Створення DI ...
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            // 2. Створюємо MainWindow
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            // ✅ ВИПРАВЛЕНО: Встановлюємо DataContext на ShellViewModel, 
            // щоб усі команди навігації та CurrentViewModel працювали.
            mainWindow.DataContext = _serviceProvider.GetRequiredService<ShellViewModel>();

            // 4. Показуємо MainWindow
            mainWindow.Show();

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // ... (Сервіси залишаються)
            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // ... (Views & ViewModels)
            services.AddSingleton<MainWindow>();

            // ❌ ВИДАЛЯЄМО: services.AddSingleton<ShellViewModel>(); 

            // ViewModel сторінок (тепер вони повинні стати Singleton або бути створені вручну)
            // Залишимо їх Transient, оскільки LoginViewModel створюється при запуску.
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<MainPageViewModel>();
            services.AddTransient<GameDetailsViewModel>();
            services.AddTransient<ProfileViewModel>();
        }
    }
}