// Файл: ViewModels/MainPageViewModel.cs

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Services;
using Presentation.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using System; // ⬅️ ВИПРАВЛЕННЯ 1: Для StringComparison
using System.Collections.Generic; // ⬅️ ВИПРАВЛЕННЯ 2: Для List<T>

namespace Presentation.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        // 1. ЗАЛЕЖНОСТІ (DI)
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;

        // 2. ВЛАСТИВОСТІ ДЛЯ ПРИВ'ЯЗКИ 
        [ObservableProperty]
        private ObservableCollection<Game> _games;

        private ObservableCollection<Game> _allGames;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Games))]
        private string _searchTerm = string.Empty;

        // 3. КОНСТРУКТОР
        public MainPageViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            _games = new ObservableCollection<Game>();
            _allGames = new ObservableCollection<Game>();

            LoadGamesAsync();
        }

        // 4. КОМАНДИ
        [RelayCommand]
        private void GoToProfile()
        {
            _navigationService.NavigateTo<ProfileViewModel>();
        }

        [RelayCommand]
        private void GoToGameDetails(Game selectedGame)
        {
            if (selectedGame != null)
            {
                _navigationService.NavigateTo<GameDetailsViewModel>();
            }
        }

        // 5. ЛОГІКА ДАНИХ ТА ФІЛЬТРАЦІЇ

        partial void OnSearchTermChanged(string value)
        {
            FilterGames(value);
        }

        private void FilterGames(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                Games = new ObservableCollection<Game>(_allGames);
                return;
            }

            var filtered = _allGames
                .Where(g => g.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Games = new ObservableCollection<Game>(filtered);
        }

        private async void LoadGamesAsync()
        {
            // Імітація завантаження даних
            await Task.Delay(100);

            // Тут має бути реальний виклик _dataService.GetGamesAsync()
            var dummyGames = new List<Game> // ⬅️ ТУТ ПОТРІБЕН List
            {
                new Game { Id = 1, Title = "Cyberpunk 2077", Price = 59.99m },
                new Game { Id = 2, Title = "The Witcher 3", Price = 19.99m },
                new Game { Id = 3, Title = "Elden Ring", Price = 49.99m },
                new Game { Id = 4, Title = "Baldur's Gate 3", Price = 69.99m },
                new Game { Id = 5, Title = "Hogwarts Legacy", Price = 59.99m },
                new Game { Id = 6, Title = "Diablo IV", Price = 70.00m },
                new Game { Id = 7, Title = "Starfield", Price = 69.99m },
                new Game { Id = 8, Title = "Red Dead Redemption 2", Price = 49.99m }
            };

            _allGames = new ObservableCollection<Game>(dummyGames);
            Games = new ObservableCollection<Game>(dummyGames);
        }
    }
}