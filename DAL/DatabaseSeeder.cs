// Файл: DAL/DatabaseSeeder.cs

using GameOverDose.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace GameOverDose.DAL
{
    public static class DatabaseSeeder
    {
        public static void Seed(GameOverDoseDbContext context)
        {
            // Перевірка чи база даних порожня
            if (context.Users.Any())
            {
                Console.WriteLine("База даних вже містить дані.");
                return;
            }

            Console.WriteLine("Заповнення бази даних тестовими даними...");

            // ========================================
            // 1. ДОДАВАННЯ КОРИСТУВАЧІВ
            // ========================================
            var users = new[]
            {
                new User
                {
                    Nickname = "Іван_Геймер",
                    Email = "ivan@gameoverdose.com",
                    Password = "hashed_password_123", // В реальному проекті хешуйте!
                    Avatar = "avatar1.png",
                    Description = "Люблю RPG та стратегії",
                    Lvl = 15
                },
                new User
                {
                    Nickname = "test@game.com",
                    Email = "test@game.com",
                    Password = "123", // Тестовий користувач для логіну
                    Avatar = "avatar2.png",
                    Description = "Професійний геймер",
                    Lvl = 25
                },
                new User
                {
                    Nickname = "Марія_Гравець",
                    Email = "maria@gameoverdose.com",
                    Password = "hashed_password_456",
                    Avatar = "avatar3.png",
                    Description = "Фанатка шутерів",
                    Lvl = 10
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            // ========================================
            // 2. ДОДАВАННЯ ІГОР
            // ========================================
            var games = new[]
            {
                new Game
                {
                    Slug = "cyberpunk-2077",
                    Name = "Cyberpunk 2077",
                    Release = new DateTime(2020, 12, 10),
                    BackgroundImage = "https://media.rawg.io/media/games/26d/26d4437715bee60138dab4a7c8c59c92.jpg",
                    Description = "Cyberpunk 2077 is an open-world, action-adventure RPG set in the megalopolis of Night City.",
                    Rating = 4.2,
                    RatingTop = 5,
                    Ratings = "exceptional",
                    RatingsCount = 15000,
                    Playtime = 60,
                    EsrbRating = "M",
                    Platforms = "PC, PS5, Xbox Series X",
                    Price = 59.99m
                },
                new Game
                {
                    Slug = "the-witcher-3",
                    Name = "The Witcher 3: Wild Hunt",
                    Release = new DateTime(2015, 5, 19),
                    BackgroundImage = "https://media.rawg.io/media/games/618/618c2031a07bbff6b4f611f10b6bcdbc.jpg",
                    Description = "The Witcher 3: Wild Hunt is a story-driven open world RPG set in a visually stunning fantasy universe.",
                    Rating = 4.8,
                    RatingTop = 5,
                    Ratings = "exceptional",
                    RatingsCount = 50000,
                    Playtime = 100,
                    EsrbRating = "M",
                    Platforms = "PC, PS4, Xbox One, Switch",
                    Price = 39.99m
                },
                new Game
                {
                    Slug = "elden-ring",
                    Name = "Elden Ring",
                    Release = new DateTime(2022, 2, 25),
                    BackgroundImage = "https://media.rawg.io/media/games/5ec/5ecac5cb026ec26a56efcc546364e348.jpg",
                    Description = "Elden Ring is an action RPG set in a world created by Hidetaka Miyazaki and George R.R. Martin.",
                    Rating = 4.7,
                    RatingTop = 5,
                    Ratings = "exceptional",
                    RatingsCount = 30000,
                    Playtime = 80,
                    EsrbRating = "M",
                    Platforms = "PC, PS5, Xbox Series X",
                    Price = 59.99m
                },
                new Game
                {
                    Slug = "minecraft",
                    Name = "Minecraft",
                    Release = new DateTime(2011, 11, 18),
                    BackgroundImage = "https://media.rawg.io/media/games/b4e/b4e4c73d5aa4ec66bbf75375c4847a2b.jpg",
                    Description = "Minecraft is a sandbox video game where players can build and explore virtual worlds.",
                    Rating = 4.5,
                    RatingTop = 5,
                    Ratings = "recommended",
                    RatingsCount = 100000,
                    Playtime = 200,
                    EsrbRating = "E10+",
                    Platforms = "PC, PS4, Xbox One, Switch, Mobile",
                    Price = 26.95m
                },
                new Game
                {
                    Slug = "gta-v",
                    Name = "Grand Theft Auto V",
                    Release = new DateTime(2013, 9, 17),
                    BackgroundImage = "https://media.rawg.io/media/games/20a/20aa03a10cda45239fe22d035c0ebe64.jpg",
                    Description = "Grand Theft Auto V is an action-adventure game set in the fictional state of San Andreas.",
                    Rating = 4.6,
                    RatingTop = 5,
                    Ratings = "exceptional",
                    RatingsCount = 80000,
                    Playtime = 70,
                    EsrbRating = "M",
                    Platforms = "PC, PS5, Xbox Series X",
                    Price = 29.99m
                }
            };

            context.Games.AddRange(games);
            context.SaveChanges();

            // ========================================
            // 3. ДОДАВАННЯ КОМЕНТАРІВ
            // ========================================
            var comments = new[]
            {
                new Comment
                {
                    UserId = users[0].Id,
                    GameId = games[0].Id,
                    Text = "Чудова гра! Атмосфера неймовірна, хоча є баги.",
                    Rating = 8,
                    CreatedAt = DateTime.Now.AddDays(-5)
                },
                new Comment
                {
                    UserId = users[1].Id,
                    GameId = games[1].Id,
                    Text = "Одна з найкращих RPG за всі часи!",
                    Rating = 10,
                    CreatedAt = DateTime.Now.AddDays(-10)
                },
                new Comment
                {
                    UserId = users[2].Id,
                    GameId = games[2].Id,
                    Text = "Складно, але дуже затягує. Графіка топ!",
                    Rating = 9,
                    CreatedAt = DateTime.Now.AddDays(-2)
                }
            };

            context.Comments.AddRange(comments);
            context.SaveChanges();

            // ========================================
            // 4. ДОДАВАННЯ ІГРОВИХ СЕСІЙ
            // ========================================
            var userGames = new[]
            {
                new UserGame
                {
                    UserId = users[0].Id,
                    GameId = games[0].Id,
                    Hours = 45,
                    Status = "playing",
                    Progress = 60,
                    PersonalRating = 8,
                    AddedAt = DateTime.Now.AddDays(-30),
                    LastPlayed = DateTime.Now.AddDays(-1)
                },
                new UserGame
                {
                    UserId = users[0].Id,
                    GameId = games[1].Id,
                    Hours = 120,
                    Status = "completed",
                    Progress = 100,
                    PersonalRating = 10,
                    IsFavorite = true,
                    AddedAt = DateTime.Now.AddDays(-180),
                    LastPlayed = DateTime.Now.AddDays(-60)
                },
                new UserGame
                {
                    UserId = users[1].Id,
                    GameId = games[2].Id,
                    Hours = 80,
                    Status = "playing",
                    Progress = 75,
                    PersonalRating = 9,
                    AddedAt = DateTime.Now.AddDays(-45),
                    LastPlayed = DateTime.Now
                },
                new UserGame
                {
                    UserId = users[2].Id,
                    GameId = games[3].Id,
                    Hours = 300,
                    Status = "playing",
                    Progress = 50,
                    PersonalRating = 9,
                    IsFavorite = true,
                    AddedAt = DateTime.Now.AddDays(-365),
                    LastPlayed = DateTime.Now.AddDays(-3)
                }
            };

            context.UserGames.AddRange(userGames);
            context.SaveChanges();

            // ========================================
            // 5. ДОДАВАННЯ ДРУЖНІХ ЗВ'ЯЗКІВ
            // ========================================
            var friends = new[]
            {
                new Friend
                {
                    UserId1 = users[0].Id,
                    UserId2 = users[1].Id,
                    Status = "accepted",
                    CreatedAt = DateTime.Now.AddDays(-90)
                },
                new Friend
                {
                    UserId1 = users[0].Id,
                    UserId2 = users[2].Id,
                    Status = "pending",
                    CreatedAt = DateTime.Now.AddDays(-5)
                },
                new Friend
                {
                    UserId1 = users[1].Id,
                    UserId2 = users[2].Id,
                    Status = "accepted",
                    CreatedAt = DateTime.Now.AddDays(-120)
                }
            };

            context.Friends.AddRange(friends);
            context.SaveChanges();

            Console.WriteLine("База даних успішно заповнена тестовими даними!");
            Console.WriteLine($"Додано: {users.Length} користувачів, {games.Length} ігор, {comments.Length} коментарів");
        }
    }
}