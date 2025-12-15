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
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GameOverDoseDbContext>();
                try
                {
                    dbContext.Database.Migrate();
                    Console.WriteLine("База даних успішно ініціалізована!");
                    DatabaseSeeder.Seed(dbContext);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка підключення до бази даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Shutdown();
                    return;
                }
            }

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetRequiredService<ShellViewModel>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<GameOverDoseDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IUserGameRepository, UserGameRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IUserGameService, UserGameService>();

            services.AddSingleton<IDataService, DataService>();

            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<NavigationService>();
            services.AddSingleton<INavigationService>(provider => provider.GetRequiredService<NavigationService>());
            services.AddSingleton<IGameNavigationService>(provider => provider.GetRequiredService<NavigationService>());

            services.AddSingleton<MainWindow>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<LoginViewModel>();
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