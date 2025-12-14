// /DAL/Interfaces/IUserRepository.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using GameOverDose.DAL.Entities;

namespace GameOverDose.DAL.Interfaces;

public interface IUserRepository
{
    // CRUD
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> GetAllAsync();
    Task<User> AddAsync(User user);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);

    // Специфічні запити
    Task<User?> GetByNicknameAsync(string nickname);
    Task<bool> ExistsByNicknameAsync(string nickname);

    // Запити для сервісу
    Task<List<User>> SearchAsync(string searchText);
    Task<List<User>> GetTopByLevelAsync(int count);

    // Запит з навігаційними властивостями
    Task<User?> GetWithGamesAsync(int id);
    Task<User?> GetWithCommentsAsync(int id);
}