// Файл: ViewModels/GameDetailsViewModel.cs

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Models;
using Presentation.Services;
using System.Collections.Generic;
using System.Windows;

namespace Presentation.ViewModels
{
    public partial class GameDetailsViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        // Властивість для відображення детальної інформації про гру
        [ObservableProperty]
        private Game currentGame;

        // Властивість для відображення графіку цін (заглушка)
        [ObservableProperty]
        private List<PriceDataPoint> priceHistory;

        // **********************************************
        // КОНСТРУКТОРИ
        // **********************************************

        // ✅ 1. БЕЗПАРАМЕТРИЧНИЙ КОНСТРУКТОР (ДЛЯ ДИЗАЙНЕРА XAML)
        public GameDetailsViewModel() : this(null)
        {
            // Ініціалізація заглушками для дизайнера
            CurrentGame = new Game
            {
                Title = "Cyberpunk 2077",
                Description = "Велика рольова гра у футуристичному Night City. Ви — V, найманець, який шукає безсмертя.",
                Developers = new List<string> { "CD Projekt RED", "Digital Scapes" },
                Price = 59.99m
            };
            LoadPriceHistory();
        }

        // ✅ 2. ОСНОВНИЙ КОНСТРУКТОР (ДЛЯ DI-КОНТЕЙНЕРА)
        public GameDetailsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Якщо потрібна логіка завантаження даних на основі ID, 
            // її слід додати тут, використовуючи сервіс даних.
        }

        // **********************************************
        // КОМАНДИ ТА МЕТОДИ
        // **********************************************

        [RelayCommand]
        private void Follow()
        {
            if (CurrentGame != null)
            {
                MessageBox.Show($"Ви стежите за грою: {CurrentGame.Title}!", "Сповіщення");
            }
        }

        // Метод-заглушка для імітації завантаження історії цін
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

    // Допоміжні класи для прикладу (історія цін)
    public class PriceDataPoint
    {
        public int Time { get; set; }
        public decimal Price { get; set; }
    }
}