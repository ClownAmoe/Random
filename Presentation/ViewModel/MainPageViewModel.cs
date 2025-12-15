// Presentation/ViewModels/MainPageViewModel.cs (ВИПРАВЛЕНО: передача ID)

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameOverDose.BLL.Interfaces;
using Presentation.Models;
using Presentation.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace Presentation.ViewModels
{
    using Game = Presentation.Models.GameModel;

    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IGameNavigationService _navigationService;
        private readonly IGameService _gameService;

        [ObservableProperty]
        private ObservableCollection<Game> _games = new();
        private List<Game> _allGames = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Games))]
        private string _searchTerm = string.Empty;

        public MainPageViewModel(IGameNavigationService navigationService, IGameService gameService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            LoadGamesAsync();
        }

        [RelayCommand]
        private void GoToGameDetails(Game selectedGame)
        {
            if (selectedGame != null && selectedGame.Id > 0)
            {
                Debug.WriteLine($"Навігація до гри ID: {selectedGame.Id}");
                _navigationService.NavigateToGameDetails(selectedGame.Id);
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

                var presentationGames = dalGames.Select(g => new Presentation.Models.GameModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    Title = g.Name,
                    Price = g.Price ?? 0m,
                    ImageSource = string.IsNullOrEmpty(g.BackgroundImage)
                        ? string.Empty
                        : g.BackgroundImage,
                    Genre = g.Platforms ?? "Unknown",
                    Description = $"Rating: {g.Rating:F1}★ | Released: {g.Release?.Year ?? 0}"
                }).ToList();

                _allGames = presentationGames;
                Games = new ObservableCollection<Game>(_allGames);

                Debug.WriteLine($"✅ Завантажено {Games.Count} ігор з бази даних");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Помилка завантаження ігор: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}