// Файл: ViewModels/MainPageViewModel.cs (ФІНАЛЬНЕ ВИПРАВЛЕННЯ CS1061)

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameOverDose.BLL.Interfaces;
using Presentation.Models;
using Presentation.Services;
using Presentation.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Presentation.ViewModels
{
    using Game = Presentation.Models.Game;

    public partial class MainPageViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IGameService _gameService;

        [ObservableProperty]
        private ObservableCollection<Game> _games = new();
        private List<Game> _allGames = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Games))]
        private string _searchTerm = string.Empty;

        public MainPageViewModel(INavigationService navigationService, IGameService gameService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            LoadGamesAsync();
        }

        [RelayCommand]
        private void GoToProfile() => _navigationService.NavigateTo<ProfileViewModel>();

        [RelayCommand]
        private void GoToGameDetails(Game selectedGame)
        {
            if (selectedGame != null)
            {
                _navigationService.NavigateTo<GameDetailsViewModel>();
            }
        }

        partial void OnSearchTermChanged(string value) => FilterGames(value);

        private void FilterGames(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                Games = new ObservableCollection<Game>(_allGames);
                return;
            }

            var filtered = _allGames
                .Where(g => g.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Games = new ObservableCollection<Game>(filtered);
        }

        private async void LoadGamesAsync()
        {
            try
            {
                var dalGames = await _gameService.GetTopRatedGamesAsync(20);

                // ✅ ВИПРАВЛЕНО CS1061: Використовуємо ТІЛЬКИ dalGame.Name
                var presentationGames = dalGames.Select(g => new Presentation.Models.Game
                {
                    Id = g.Id,
                    Name = g.Name,
                    Title = g.Name, // <-- ВИКОРИСТОВУЄМО ТІЛЬКИ g.Name
                    Price = 0m, // <-- Присвоюємо 0m, оскільки Price відсутній у DAL
                    // ... мапінг інших властивостей ...
                }).ToList();

                _allGames = presentationGames;
                Games = new ObservableCollection<Game>(_allGames);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Помилка завантаження ігор: {ex.Message}");
            }
        }
    }
}