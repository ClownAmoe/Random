// /BLL/Interfaces/ICommentService.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using GameOverDose.DAL.Entities; // <-- Використовуємо DAL-сутність Comment

namespace GameOverDose.BLL.Interfaces; // <-- Новий простір імен BLL

/// <summary>
/// Інтерфейс сервісу для управління коментарями
/// </summary>
public interface ICommentService
{
    /// <summary>
    /// Отримує всі коментарі
    /// </summary>
    Task<List<Comment>> GetAllCommentsAsync();

    /// <summary>
    /// Отримує коментар за ID
    /// </summary>
    Task<Comment?> GetCommentByIdAsync(int id); // Nullable, оскільки коментар може не існувати

    /// <summary>
    /// Отримує коментарі до гри
    /// </summary>
    Task<List<Comment>> GetCommentsByGameAsync(int gameId);

    /// <summary>
    /// Отримує коментарі користувача
    /// </summary>
    Task<List<Comment>> GetCommentsByUserAsync(int userId);

    /// <summary>
    /// Створює новий коментар
    /// </summary>
    Task<Comment> CreateCommentAsync(Comment comment);

    /// <summary>
    /// Оновлює коментар
    /// </summary>
    Task<bool> UpdateCommentAsync(Comment comment);

    /// <summary>
    /// Видаляє коментар
    /// </summary>
    Task<bool> DeleteCommentAsync(int id);

    /// <summary>
    /// Отримує останні коментарі
    /// </summary>
    Task<List<Comment>> GetRecentCommentsAsync(int count);

    /// <summary>
    /// Отримує позитивні коментарі (рейтинг >= 7)
    /// </summary>
    Task<List<Comment>> GetPositiveCommentsAsync();

    /// <summary>
    /// Отримує негативні коментарі (рейтинг <= 4)
    /// </summary>
    Task<List<Comment>> GetNegativeCommentsAsync();

    /// <summary>
    /// Розраховує середній рейтинг для гри
    /// </summary>
    Task<double?> GetAverageRatingForGameAsync(int gameId); // Nullable, якщо коментарів немає

    /// <summary>
    /// Отримує кількість коментарів для гри
    /// </summary>
    Task<int> GetCommentCountForGameAsync(int gameId);

    /// <summary>
    /// Перевіряє чи користувач вже коментував цю гру
    /// </summary>
    Task<bool> HasUserCommentedGameAsync(int userId, int gameId);
}