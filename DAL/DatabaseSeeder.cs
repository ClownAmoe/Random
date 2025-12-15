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
            if (context.Users.Any())
            {
                Console.WriteLine("База даних вже містить дані.");

                // ✅ ДОДАНО: Оновлення трейлерів для існуючих ігор
                UpdateTrailers(context);
                return;
            }

            Console.WriteLine("Заповнення бази даних тестовими даними...");

            // ... (решта коду залишається без змін)
            var users = new[]
            {
                new User
                {
                    Nickname = "Іван_Геймер",
                    Email = "ivan@gameoverdose.com",
                    Password = "hashed_password_123",
                    Avatar = "avatar1.png",
                    Description = "Люблю RPG та стратегії",
                    Lvl = 15
                },
                new User
                {
                    Nickname = "test@game.com",
                    Email = "test@game.com",
                    Password = "123",
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
                    Price = 59.99m,
                    TrailerUrl = "https://www.youtube.com/embed/8X2kIfS6fb8"
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
                    Price = 39.99m,
                    TrailerUrl = "https://www.youtube.com/embed/c0i88t0Kacs"
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
                    Price = 59.99m,
                    TrailerUrl = "https://www.youtube.com/embed/E3Huy2cdih0"
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
                    Price = 26.95m,
                    TrailerUrl = "https://www.youtube.com/embed/MmB9b5njVbA"
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
                    Price = 29.99m,
                    TrailerUrl = "https://www.youtube.com/embed/QkkoHAzjnUs"
                }
            };

            context.Games.AddRange(games);
            context.SaveChanges();

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

        // ✅ НОВИЙ МЕТОД: Оновлення трейлерів для існуючих ігор
        private static void UpdateTrailers(GameOverDoseDbContext context)
        {
            var trailerUpdates = new Dictionary<string, string>
            {
                { "cyberpunk-2077", "https://www.youtube.com/embed/8X2kIfS6fb8" },
                { "the-witcher-3", "https://www.youtube.com/embed/c0i88t0Kacs" },
                { "elden-ring", "https://www.youtube.com/embed/E3Huy2cdih0" },
                { "minecraft", "https://www.youtube.com/embed/MmB9b5njVbA" },
                { "gta-v", "https://www.youtube.com/embed/QkkoHAzjnUs" }
            };

            bool hasUpdates = false;

            foreach (var update in trailerUpdates)
            {
                var game = context.Games.FirstOrDefault(g => g.Slug == update.Key);

                if (game != null && string.IsNullOrEmpty(game.TrailerUrl))
                {
                    game.TrailerUrl = update.Value;
                    hasUpdates = true;
                    Console.WriteLine($"✅ Додано трейлер для гри: {game.Name}");
                }
            }

            if (hasUpdates)
            {
                context.SaveChanges();
                Console.WriteLine("✅ Трейлери успішно оновлено!");
            }
            else
            {
                Console.WriteLine("ℹ️ Всі трейлери вже присутні в базі даних.");
            }
        }
    }
}