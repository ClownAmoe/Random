// /BLL/Interfaces/IUserService.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using GameOverDose.DAL.Entities; // <-- Використовуємо DAL-сутність User

namespace GameOverDose.BLL.Interfaces;

/// <summary>
/// Інтерфейс сервісу для управління користувачами (BLL)
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Отримує всіх користувачів
    /// </summary>
    Task<List<User>> GetAllUsersAsync();

    /// <summary>
    /// Отримує користувача за ID
    /// </summary>
    Task<User?> GetUserByIdAsync(int id); // Nullable, оскільки користувач може не існувати

    /// <summary>
    /// Отримує користувача за нікнеймом
    /// </summary>
    Task<User?> GetUserByNicknameAsync(string nickname); // Nullable

    /// <summary>
    /// Створює нового користувача
    /// </summary>
    Task<User> CreateUserAsync(User user);

    /// <summary>
    /// Оновлює дані користувача
    /// </summary>
    Task<bool> UpdateUserAsync(User user);

    /// <summary>
    /// Видаляє користувача
    /// </summary>
    Task<bool> DeleteUserAsync(int id);

    /// <summary>
    /// Шукає користувачів за текстом (nickname або email)
    /// </summary>
    Task<List<User>> SearchUsersAsync(string searchText);

    /// <summary>
    /// Отримує топ користувачів за рівнем
    /// </summary>
    Task<List<User>> GetTopUsersByLevelAsync(int count);

    /// <summary>
    /// Перевіряє чи існує користувач з таким нікнеймом
    /// </summary>
    Task<bool> UserExistsAsync(string nickname);

    /// <summary>
    /// Отримує користувача з його іграми (завантажуючи UserGames)
    /// </summary>
    Task<User?> GetUserWithGamesAsync(int id);

    /// <summary>
    /// Отримує користувача з його коментарями (завантажуючи Comments)
    /// </summary>
    Task<User?> GetUserWithCommentsAsync(int id);
}