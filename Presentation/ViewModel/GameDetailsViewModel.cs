// Файл: ViewModels/GameDetailsViewModel.cs (ВИПРАВЛЕНО: коментарі та зображення)

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Models;
using Presentation.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System;

namespace Presentation.ViewModels
{
    public partial class GameDetailsViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private Game currentGame;

        [ObservableProperty]
        private List<PriceDataPoint> priceHistory;

        [ObservableProperty]
        private ObservableCollection<GameComment> comments = new();

        [ObservableProperty]
        private string newCommentText = string.Empty;

        [ObservableProperty]
        private int newCommentRating = 5;

        [ObservableProperty]
        private bool isTracking = false;

        [ObservableProperty]
        private ObservableCollection<RatingStar> ratingStars = new();

        [ObservableProperty]
        private string achievementsProgress = "0/50";

        [ObservableProperty]
        private ObservableCollection<Achievement> achievementsList = new();

        // **********************************************
        // КОНСТРУКТОРИ
        // **********************************************

        public GameDetailsViewModel() : this(null)
        {
            // Ініціалізація заглушками для дизайнера
            CurrentGame = new Game
            {
                Title = "Cyberpunk 2077",
                Description = "Велика рольова гра у футуристичному Night City. Ви — V, найманець, який шукає безсмертя.",
                Developers = new List<string> { "CD Projekt RED", "Digital Scapes" },
                Price = 59.99m,
                ImageSource = "cyberpunk_bg.jpg"
            };
            LoadPriceHistory();
            InitializeRatingStars();
            LoadMockComments();
            LoadMockAchievements();
        }

        public GameDetailsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            InitializeRatingStars();
            LoadMockAchievements();
        }

        // **********************************************
        // ІНІЦІАЛІЗАЦІЯ
        // **********************************************

        private void InitializeRatingStars()
        {
            RatingStars.Clear();
            for (int i = 1; i <= 10; i++)
            {
                RatingStars.Add(new RatingStar
                {
                    Value = i,
                    Color = i <= NewCommentRating ? "#FFC830" : "#444444"
                });
            }
        }

        private void LoadMockComments()
        {
            Comments = new ObservableCollection<GameComment>
            {
                new GameComment
                {
                    Author = "Іван_Геймер",
                    Text = "Чудова гра! Атмосфера неймовірна, хоча є баги.",
                    Date = DateTime.Now.AddDays(-5),
                    Rating = 8
                },
                new GameComment
                {
                    Author = "Марія_Гравець",
                    Text = "Графіка вражає, але оптимізація хромає.",
                    Date = DateTime.Now.AddDays(-2),
                    Rating = 7
                }
            };
        }

        private void LoadMockAchievements()
        {
            AchievementsList = new ObservableCollection<Achievement>
            {
                new Achievement { Name = "Перша кров", Description = "Виконайте перше завдання", StatusColor = "#10AA10" },
                new Achievement { Name = "Легенда Night City", Description = "Досягніть максимального рівня", StatusColor = "#444444" },
                new Achievement { Name = "Майстер взлому", Description = "Зламайте 50 пристроїв", StatusColor = "#444444" }
            };
            AchievementsProgress = $"1/{AchievementsList.Count}";
        }

        // **********************************************
        // КОМАНДИ
        // **********************************************

        [RelayCommand]
        private void Follow()
        {
            if (CurrentGame != null)
            {
                MessageBox.Show($"Ви стежите за грою: {CurrentGame.Title}!", "Сповіщення");
            }
        }

        [RelayCommand]
        private void ToggleTracking()
        {
            IsTracking = !IsTracking;
            MessageBox.Show(
                IsTracking ? "Відстеження розпочато!" : "Відстеження зупинено!",
                "Tracking",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        [RelayCommand]
        private void SetRating(int rating)
        {
            NewCommentRating = rating;

            // Оновлюємо кольори зірок
            for (int i = 0; i < RatingStars.Count; i++)
            {
                RatingStars[i].Color = (i + 1) <= rating ? "#FFC830" : "#444444";
            }
        }

        [RelayCommand]
        private void PostComment()
        {
            if (string.IsNullOrWhiteSpace(NewCommentText))
            {
                MessageBox.Show("Введіть текст коментаря", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // ✅ ДОДАВАННЯ НОВОГО КОМЕНТАРЯ
            var newComment = new GameComment
            {
                Author = "Поточний користувач", // TODO: Замінити на реального користувача
                Text = NewCommentText,
                Date = DateTime.Now,
                Rating = NewCommentRating
            };

            Comments.Insert(0, newComment); // Додаємо на початок списку

            // Очищуємо форму
            NewCommentText = string.Empty;
            NewCommentRating = 5;
            InitializeRatingStars();

            MessageBox.Show("Коментар додано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadPriceHistory()
        {
            PriceHistory = new List<PriceDataPoint>
            {
                new PriceDataPoint { Time = 1, Price = 59.99m },
                new PriceDataPoint { Time = 2, Price = 49.99m },
                new PriceDataPoint { Time = 3, Price = 54.99m },
                new PriceDataPoint { Time = 4, Price = 59.99m }
            };
        }
    }

    // **********************************************
    // ДОПОМІЖНІ КЛАСИ
    // **********************************************

    public class PriceDataPoint
    {
        public int Time { get; set; }
        public decimal Price { get; set; }
    }

    public class GameComment
    {
        public string Author { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int Rating { get; set; }
    }

    public partial class RatingStar : ObservableObject
    {
        [ObservableProperty]
        private int value;

        [ObservableProperty]
        private string color = "#444444";
    }

    public class Achievement
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StatusColor { get; set; } = "#444444";
    }
}