// /BLL/Interfaces/IGameService.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using GameOverDose.DAL.Entities; // <-- Використовуємо DAL-сутність Game

namespace GameOverDose.BLL.Interfaces; // <-- Новий простір імен BLL

/// <summary>
/// Інтерфейс сервісу для управління іграми
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Отримує всі ігри
    /// </summary>
    Task<List<Game>> GetAllGamesAsync();

    /// <summary>
    /// Отримує гру за ID
    /// </summary>
    Task<Game?> GetGameByIdAsync(int id); // Nullable

    /// <summary>
    /// Отримує гру за slug
    /// </summary>
    Task<Game?> GetGameBySlugAsync(string slug); // Nullable

    /// <summary>
    /// Створює нову гру
    /// </summary>
    Task<Game> CreateGameAsync(Game game);

    /// <summary>
    /// Оновлює дані гри
    /// </summary>
    Task<bool> UpdateGameAsync(Game game);

    /// <summary>
    /// Видаляє гру
    /// </summary>
    Task<bool> DeleteGameAsync(int id);

    /// <summary>
    /// Шукає ігри за назвою або slug
    /// </summary>
    Task<List<Game>> SearchGamesAsync(string searchText);

    /// <summary>
    /// Отримує топ ігор за рейтингом
    /// </summary>
    Task<List<Game>> GetTopRatedGamesAsync(int count);

    /// <summary>
    /// Отримує нові ігри (вийшли протягом останнього року)
    /// </summary>
    Task<List<Game>> GetNewGamesAsync();

    /// <summary>
    /// Отримує популярні ігри (за кількістю гравців)
    /// </summary>
    Task<List<Game>> GetPopularGamesAsync(int count);

    /// <summary>
    /// Отримує гру з коментарями
    /// </summary>
    Task<Game?> GetGameWithCommentsAsync(int id);

    /// <summary>
    /// Перевіряє чи існує гра з таким slug
    /// </summary>
    Task<bool> GameExistsAsync(string slug);

    /// <summary>
    /// Отримує ігри за платформою
    /// </summary>
    Task<List<Game>> GetGamesByPlatformAsync(string platform);

    /// <summary>
    /// Розраховує середній рейтинг гри на основі коментарів
    /// </summary>
    Task<double?> GetAverageRatingFromCommentsAsync(int gameId); // Nullable, якщо немає коментарів
}