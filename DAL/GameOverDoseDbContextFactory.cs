// Файл: DAL/GameOverDoseDbContextFactory.cs
// Design-Time Factory для EF Core Tools

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GameOverDose.DAL
{
    /// <summary>
    /// Фабрика для створення DbContext під час виконання міграцій
    /// </summary>
    public class GameOverDoseDbContextFactory : IDesignTimeDbContextFactory<GameOverDoseDbContext>
    {
        public GameOverDoseDbContext CreateDbContext(string[] args)
        {
            // Шлях до appsettings.json у проекті Presentation
            var presentationPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Presentation");

            // Якщо файл не знайдено в стандартному місці, шукаємо в поточній директорії
            if (!Directory.Exists(presentationPath))
            {
                presentationPath = Directory.GetCurrentDirectory();
            }

            // Завантаження конфігурації
            var configuration = new ConfigurationBuilder()
                .SetBasePath(presentationPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Отримання рядка підключення
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Якщо рядок підключення не знайдено, використовуємо значення за замовчуванням
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Host=localhost;Port=5432;Database=GameOverDoseDB;Username=postgres;Password=123123xdd";
                System.Console.WriteLine("⚠️ УВАГА: Використовується рядок підключення за замовчуванням!");
                System.Console.WriteLine($"Рядок підключення: {connectionString}");
            }

            // Створення опцій для DbContext
            var optionsBuilder = new DbContextOptionsBuilder<GameOverDoseDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            // Логування (опціонально)
            System.Console.WriteLine($"✅ Підключення до бази: {connectionString.Replace(connectionString.Split(';')[3], "Password=***")}");

            return new GameOverDoseDbContext(optionsBuilder.Options);
        }
    }
}