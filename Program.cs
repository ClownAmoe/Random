using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

// Всі ваші простори імен для реєстрації
using GameOverDose.DAL;
using GameOverDose.DAL.Interfaces;
using GameOverDose.DAL.Repositories;
using GameOverDose.BLL.Interfaces;
using GameOverDose.BLL.Services;

// Головний клас-запуск (мінімалістичний хостинг .NET 6+)
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// ===============================================
// 1. КОНФІГУРАЦІЯ БАЗИ ДАНИХ ТА DB CONTEXT (DAL)
// ===============================================

// Примітка: Для реального застосунку необхідно додати файл appsettings.json
// та встановити пакет Microsoft.EntityFrameworkCore.SqlServer
builder.Services.AddDbContext<GameOverDoseDbContext>(options =>
    // Приклад використання MSSQL (замініть на ваш рядок підключення)
    // Рядок підключення зазвичай береться з appsettings.json
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                         ?? "Server=(localdb)\\mssqllocaldb;Database=GameOverDoseDB;Trusted_Connection=True;"));


// ===============================================
// 2. РЕЄСТРАЦІЯ РЕПОЗИТОРІЇВ (DAL)
// ===============================================
// Реєструємо репозиторії як Scoped, оскільки вони залежать від DbContext
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Додайте інші реалізовані репозиторії
// builder.Services.AddScoped<IGameRepository, GameRepository>();
// builder.Services.AddScoped<ICommentRepository, CommentRepository>();
// builder.Services.AddScoped<IUserGameRepository, UserGameRepository>();


// ===============================================
// 3. РЕЄСТРАЦІЯ СЕРВІСІВ (BLL)
// ===============================================
// Реєструємо сервіси як Scoped (зазвичай), оскільки вони використовують Scoped репозиторії
builder.Services.AddScoped<IUserService, UserService>();
// Додайте інші сервіси
// builder.Services.AddScoped<IGameService, GameService>();
// builder.Services.AddScoped<ICommentService, CommentService>();
// builder.Services.AddScoped<IUserGameService, UserGameService>();


// ===============================================
// 4. ЗАПУСК ЗАСТОСУНКУ
// ===============================================
IHost host = builder.Build();

// Приклад використання сервісу після налаштування
using (var scope = host.Services.CreateScope())
{
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

    Console.WriteLine("--- Тестування UserService ---");

    // Спробуйте отримати користувачів (якщо БД налаштована)
    // var users = await userService.GetAllUsersAsync();
    // Console.WriteLine($"Знайдено користувачів: {users.Count}");

    Console.WriteLine("Хост запущено та готовий до роботи.");
}

await host.RunAsync();