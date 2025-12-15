// /BLL/Interfaces/IUserGameService.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using GameOverDose.DAL.Entities; // <-- Використовуємо DAL-сутність UserGame

namespace GameOverDose.BLL.Interfaces; // <-- Новий простір імен BLL

/// <summary>
/// Інтерфейс сервісу для управління ігровими сесіями користувачів
/// </summary>
public interface IUserGameService
{
    /// <summary>
    /// Отримує всі ігрові сесії користувача
    /// </summary>
    // Примітка: Рекомендується використовувати UserId/GameId (int) замість Nickname/Slug (string)
    // на рівні сервісу, але зберігаємо поточну сигнатуру.
    Task<List<UserGame>> GetUserGamesAsync(string userNickname);

    /// <summary>
    /// Отримує гравців певної гри
    /// </summary>
    Task<List<UserGame>> GetGamePlayersAsync(string gameSlug);

    /// <summary>
    /// Отримує ігрову сесію за ID
    /// </summary>
    Task<UserGame?> GetUserGameByIdAsync(int id); // Додаємо для повноти

    /// <summary>
    /// Створює нову ігрову сесію
    /// </summary>
    Task<UserGame> CreateUserGameAsync(UserGame userGame);

    /// <summary>
    /// Оновлює ігрову сесію
    /// </summary>
    Task<bool> UpdateUserGameAsync(UserGame userGame);

    /// <summary>
    /// Видаляє ігрову сесію
    /// </summary>
    Task<bool> DeleteUserGameAsync(int id);

    /// <summary>
    /// Додає години до ігрової сесії
    /// </summary>
    Task<bool> AddPlaytimeAsync(int userGameId, int hours);

    /// <summary>
    /// Оновлює прогрес гри
    /// </summary>
    Task<bool> UpdateProgressAsync(int userGameId, int progress);

    /// <summary>
    /// Позначає гру як завершену
    /// </summary>
    Task<bool> MarkAsCompletedAsync(int userGameId);

    /// <summary>
    /// Отримує загальний час гри користувача
    /// </summary>
    Task<int> GetTotalPlaytimeAsync(string userNickname);

    /// <summary>
    /// Отримує топ ігор користувача за часом
    /// </summary>
    Task<List<UserGame>> GetTopGamesByPlaytimeAsync(string userNickname, int count);

    /// <summary>
    /// Отримує список бажань користувача
    /// </summary>
    Task<List<UserGame>> GetWishlistAsync(string userNickname);

    /// <summary>
    /// Отримує активні ігри користувача
    /// </summary>
    Task<List<UserGame>> GetActiveGamesAsync(string userNickname);

    /// <summary>
    /// Отримує завершені ігри користувача
    /// </summary>
    Task<List<UserGame>> GetCompletedGamesAsync(string userNickname);

    Task<bool> UpdateTrackingStatusAsync(int userId, int gameId, bool isTracking);

    Task<bool> GetTrackingStatusAsync(int userId, int gameId);
}