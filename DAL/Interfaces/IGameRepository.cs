// /DAL/Interfaces/IGameRepository.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using GameOverDose.DAL.Entities;

namespace GameOverDose.DAL.Interfaces;

public interface IGameRepository
{
    // CRUD
    Task<Game?> GetByIdAsync(int id);
    Task<List<Game>> GetAllAsync();
    Task<Game> AddAsync(Game game);
    Task<bool> UpdateAsync(Game game);
    Task<bool> DeleteAsync(int id);

    // Специфічні запити
    Task<Game?> GetBySlugAsync(string slug);
    Task<bool> ExistsBySlugAsync(string slug);
    Task<List<Game>> SearchAsync(string searchText);
    Task<List<Game>> GetTopRatedAsync(int count);

    // Запити для сервісу
    Task<List<Game>> GetNewReleasesAsync();
    Task<List<Game>> GetByPlatformAsync(string platform);
    Task<Game?> GetWithCommentsAsync(int id);
    Task<Game?> GetGameWithCommentsAndUsersAsync(int id);

}