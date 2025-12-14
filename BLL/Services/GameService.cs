using GameOverDose.BLL.Interfaces;
using GameOverDose.DAL.Entities;
using GameOverDose.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // Потрібен для Where та ToList

namespace GameOverDose.BLL.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;

    // 💡 Примітка: Для методу GetAverageRatingFromCommentsAsync 
    // потрібна була б інжекція ICommentRepository, але для збірки 
    // ми поки що обійдемося імітацією.

    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    // ========================================
    // CRUD та Основні Запити
    // ========================================

    public Task<List<Game>> GetAllGamesAsync()
    {
        return _gameRepository.GetAllAsync();
    }

    public Task<Game?> GetGameByIdAsync(int id)
    {
        return _gameRepository.GetByIdAsync(id);
    }

    public Task<Game?> GetGameBySlugAsync(string slug)
    {
        return _gameRepository.GetBySlugAsync(slug);
    }

    public Task<Game> CreateGameAsync(Game game)
    {
        // Додайте тут бізнес-логіку перед збереженням
        return _gameRepository.AddAsync(game);
    }

    public Task<bool> UpdateGameAsync(Game game)
    {
        // Додайте тут бізнес-логіку перед оновленням
        return _gameRepository.UpdateAsync(game);
    }

    public Task<bool> DeleteGameAsync(int id)
    {
        // Додайте тут бізнес-логіку перед видаленням
        return _gameRepository.DeleteAsync(id);
    }

    public async Task<bool> GameExistsAsync(string slug)
    {
        var game = await _gameRepository.GetBySlugAsync(slug);
        return game != null;
    }

    // ========================================
    // Специфічні Методи та Бізнес-Логіка
    // ========================================

    // ✅ ВИПРАВЛЕНО: Додано SearchGamesAsync (Виправлення CS0535)
    public Task<List<Game>> SearchGamesAsync(string searchText)
    {
        return _gameRepository.SearchAsync(searchText);
    }

    // ✅ ВИПРАВЛЕНО: Додано GetTopRatedGamesAsync (Виправлення CS0535)
    public Task<List<Game>> GetTopRatedGamesAsync(int count)
    {
        // Використовуємо GetTopRatedAsync, реалізований у GameRepository
        return _gameRepository.GetTopRatedAsync(count);
    }

    public async Task<List<Game>> GetNewGamesAsync()
    {
        var allGames = await _gameRepository.GetAllAsync();
        // Використовуємо метод-розширення IsNewRelease
        return allGames.Where(g => g.IsNewRelease()).ToList();
    }

    public Task<List<Game>> GetPopularGamesAsync(int count)
    {
        // Популярні = Top Rated (зазвичай це одне й те саме)
        return _gameRepository.GetTopRatedAsync(count);
    }

    public Task<Game?> GetGameWithCommentsAsync(int id)
    {
        // Припускаємо, що цей метод повертає гру разом із навігаційними властивостями Comments
        return _gameRepository.GetGameWithCommentsAndUsersAsync(id);
    }

    public Task<List<Game>> GetGamesByPlatformAsync(string platformName)
    {
        // Припускаємо, що цей метод є в IGameRepository
        return _gameRepository.GetByPlatformAsync(platformName);
    }

    // ✅ ВИПРАВЛЕНО: Змінено тип повернення на Task<double?> (Виправлення CS0738)
    public async Task<double?> GetAverageRatingFromCommentsAsync(int gameId)
    {
        // Ця логіка імітує отримання середнього рейтингу,
        // поки не буде реалізований ICommentRepository
        var game = await _gameRepository.GetByIdAsync(gameId);

        return game?.Rating;
    }
}