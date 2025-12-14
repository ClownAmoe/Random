// /DAL/Interfaces/ICommentRepository.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using GameOverDose.DAL.Entities;

namespace GameOverDose.DAL.Interfaces;

public interface ICommentRepository
{
    // CRUD
    Task<Comment?> GetByIdAsync(int id);
    Task<List<Comment>> GetAllAsync();
    Task<Comment> AddAsync(Comment comment);
    Task<bool> UpdateAsync(Comment comment);
    Task<bool> DeleteAsync(int id);

    // Специфічні запити
    Task<List<Comment>> GetByGameIdAsync(int gameId);
    Task<List<Comment>> GetByUserIdAsync(int userId);
    Task<List<Comment>> GetRecentAsync(int count);
    Task<List<Comment>> GetByRatingRangeAsync(int minRating, int maxRating);

    // Запити для агрегації
    Task<double?> GetAverageRatingByGameIdAsync(int gameId);
    Task<int> GetCountByGameIdAsync(int gameId);
    Task<bool> HasUserCommentedGameAsync(int userId, int gameId);
}