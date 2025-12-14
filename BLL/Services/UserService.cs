using GameOverDose.BLL.Interfaces;
using GameOverDose.DAL.Entities;
using GameOverDose.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameOverDose.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    // Інжекція залежностей (Repository Pattern)
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        return _userRepository.GetAllAsync();
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        return _userRepository.GetByIdAsync(id);
    }

    public Task<User?> GetUserByNicknameAsync(string nickname)
    {
        return _userRepository.GetByNicknameAsync(nickname);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        // Тут повинна бути бізнес-логіка:
        // 1. Перевірка на унікальність нікнейму та email.
        // 2. Хешування пароля (КРИТИЧНО! Не зберігайте паролі відкритим текстом).

        if (await _userRepository.ExistsByNicknameAsync(user.Nickname))
        {
            // У реальному житті кидаємо виняток або повертаємо спеціальний DTO з помилкою
            throw new ArgumentException("Користувач з таким нікнеймом вже існує.");
        }

        // Тут ми припускаємо, що хешування відбувається перед викликом цього методу
        return await _userRepository.AddAsync(user);
    }

    public Task<bool> UpdateUserAsync(User user)
    {
        // Бізнес-логіка: перевірка чи користувач має право на оновлення
        return _userRepository.UpdateAsync(user);
    }

    public Task<bool> DeleteUserAsync(int id)
    {
        // Бізнес-логіка: очищення пов'язаних даних (друзі, коментарі, ігри)
        return _userRepository.DeleteAsync(id);
    }

    public Task<List<User>> SearchUsersAsync(string searchText)
    {
        return _userRepository.SearchAsync(searchText);
    }

    public Task<List<User>> GetTopUsersByLevelAsync(int count)
    {
        return _userRepository.GetTopByLevelAsync(count);
    }

    public Task<bool> UserExistsAsync(string nickname)
    {
        return _userRepository.ExistsByNicknameAsync(nickname);
    }

    public Task<User?> GetUserWithGamesAsync(int id)
    {
        return _userRepository.GetWithGamesAsync(id);
    }

    public Task<User?> GetUserWithCommentsAsync(int id)
    {
        return _userRepository.GetWithCommentsAsync(id);
    }
}