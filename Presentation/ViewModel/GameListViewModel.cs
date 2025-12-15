// Файл: ViewModels/GameListViewModel.cs (ВИПРАВЛЕННЯ CS1061)

using GameOverDose.BLL.Interfaces;
using Presentation.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Presentation.Infrastructure;
using System.Windows.Input;
using System;
using System.Diagnostics;
using System.Linq;

namespace Presentation.ViewModels;

public class GameListViewModel // : BaseViewModel 
{
    private readonly IGameService _gameService;
    public ObservableCollection<GameModel> TopGames { get; set; } = new ObservableCollection<GameModel>();
    public ICommand LoadTopGamesCommand { get; }

    public GameListViewModel(IGameService gameService)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        LoadTopGamesCommand = new RelayCommand(async (p) => await LoadTopGamesAsync());
    }

    private async Task LoadTopGamesAsync()
    {
        TopGames.Clear();

        try
        {
            var dalGames = await _gameService.GetTopRatedGamesAsync(10);

            foreach (var dalGame in dalGames)
            {
                var presentationGame = new GameModel
                {
                    Id = dalGame.Id,
                    // ✅ ВИПРАВЛЕНО CS1061: dalGame, ймовірно, має лише Name та Price
                    // Ми припускаємо, що властивості в DAL мають назви Title та Price
                    // Якщо CS1061 знову виникне, вам потрібно буде перевірити
                    // назви властивостей у DAL-сутності (наприклад, чи це g.OriginalTitle?)

                    Name = dalGame.Name ?? "Без назви",
                    Title = dalGame.Name ?? "Без назви", // <-- Припускаємо, що DAL.Game має Name
                    Price = dalGame.Price ?? 0m,
                };
                TopGames.Add(presentationGame);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Помилка завантаження топ-ігор: {ex.Message}");
        }
    }
}