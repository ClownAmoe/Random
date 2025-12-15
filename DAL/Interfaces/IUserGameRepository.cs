// DAL/Interfaces/IUserGameRepository.cs (НОВИЙ ФАЙЛ)

using System.Collections.Generic;
using System.Threading.Tasks;
using GameOverDose.DAL.Entities;

namespace GameOverDose.DAL.Interfaces
{
    public interface IUserGameRepository
    {
        // CRUD
        Task<UserGame?> GetByIdAsync(int id);
        Task<List<UserGame>> GetAllAsync();
        Task<UserGame> AddAsync(UserGame userGame);
        Task<bool> UpdateAsync(UserGame userGame);
        Task<bool> DeleteAsync(int id);

        // Специфічні запити
        Task<List<UserGame>> GetByUserIdAsync(int userId);
        Task<List<UserGame>> GetByGameIdAsync(int gameId);
        Task<int> GetTotalPlaytimeByUserAsync(int userId);
    }
}