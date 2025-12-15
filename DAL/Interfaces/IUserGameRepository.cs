using System.Collections.Generic;
using System.Threading.Tasks;
using GameOverDose.DAL.Entities;

namespace GameOverDose.DAL.Interfaces
{
    public interface IUserGameRepository
    {
        Task<UserGame?> GetByIdAsync(int id);
        Task<List<UserGame>> GetAllAsync();
        Task<UserGame> AddAsync(UserGame userGame);
        Task<bool> UpdateAsync(UserGame userGame);
        Task<bool> DeleteAsync(int id);

        Task<List<UserGame>> GetByUserIdAsync(int userId);
        Task<List<UserGame>> GetByGameIdAsync(int gameId);
        Task<UserGame?> GetByUserAndGameAsync(int userId, int gameId);

        Task<int> GetTotalPlaytimeByUserAsync(int userId);
        Task<List<UserGame>> GetTopGamesByPlaytimeAsync(int userId, int count);
        Task<List<UserGame>> GetByStatusAsync(int userId, string status);
        Task<bool> UpdateTrackingStatusAsync(int userId, int gameId, bool isTracking);
    }
}