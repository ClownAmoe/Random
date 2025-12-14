// Файл: App.xaml.cs (Повністю виправлений з підключенням до PostgreSQL)

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Windows;
using System.IO;
using Presentation.Services;
using Microsoft.EntityFrameworkCore;
using Presentation.ViewModels;
using GameOverDose.BLL.Interfaces;
using GameOverDose.DAL.Interfaces;
using GameOverDose.DAL.Repositories;
using GameOverDose.DAL;
using GameOverDose.BLL.Services;
using System;

namespace Presentation
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        private IConfiguration _configuration;

        protected override void OnStartup(StartupEventArgs e)
        {
            // 1. Завантаження конфігурації з appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();

            // 2. Створення DI-контейнера
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            // 3. Ініціалізація бази даних (застосування міграцій + seed)
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GameOverDoseDbContext>();
                try
                {
                    dbContext.Database.Migrate();
                    Console.WriteLine("База даних успішно ініціалізована!");

                    // Заповнення тестовими даними
                    DatabaseSeeder.Seed(dbContext);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка підключення до бази даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Shutdown();
                    return;
                }
            }

            // 4. Створення MainWindow
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetRequiredService<ShellViewModel>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // ========================================
            // 1. КОНФІГУРАЦІЯ БАЗИ ДАНИХ (PostgreSQL)
            // ========================================
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<GameOverDoseDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
                options.EnableSensitiveDataLogging(); // Для дебагу (вимкніть у продакшені)
            });

            // ========================================
            // 2. РЕЄСТРАЦІЯ РЕПОЗИТОРІЇВ (DAL)
            // ========================================
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            // Додайте інші репозиторії за потреби:
            // services.AddScoped<ICommentRepository, CommentRepository>();
            // services.AddScoped<IUserGameRepository, UserGameRepository>();

            // ========================================
            // 3. РЕЄСТРАЦІЯ СЕРВІСІВ (BLL)
            // ========================================
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGameService, GameService>();
            // Додайте інші сервіси за потреби:
            // services.AddScoped<ICommentService, CommentService>();
            // services.AddScoped<IUserGameService, UserGameService>();

            // ========================================
            // 4. РЕЄСТРАЦІЯ СЕРВІСІВ ПРЕЗЕНТАЦІЙНОГО ШАРУ
            // ========================================
            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // ========================================
            // 5. РЕЄСТРАЦІЯ VIEWMODELS
            // ========================================
            services.AddSingleton<MainWindow>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<LoginViewModel>(); // Тепер використовує IUserService
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<MainPageViewModel>();
            services.AddTransient<GameDetailsViewModel>();
            services.AddTransient<ProfileViewModel>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
            base.OnExit(e);
        }
    }
}