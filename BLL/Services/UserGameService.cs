// BLL/Services/UserGameService.cs (НОВИЙ ФАЙЛ)

using GameOverDose.BLL.Interfaces;
using GameOverDose.DAL.Entities;
using GameOverDose.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOverDose.BLL.Services
{
    public class UserGameService : IUserGameService
    {
        private readonly IUserGameRepository _userGameRepository;

        public UserGameService(IUserGameRepository userGameRepository)
        {
            _userGameRepository = userGameRepository;
        }

        // CRUD методи
        public Task<List<UserGame>> GetUserGamesAsync(string userNickname)
        {
            // Примітка: потрібно додати метод пошуку за nickname у репозиторії
            // Поки що повертаємо порожній список
            return Task.FromResult(new List<UserGame>());
        }

        public Task<List<UserGame>> GetGamePlayersAsync(string gameSlug)
        {
            return Task.FromResult(new List<UserGame>());
        }

        public Task<UserGame?> GetUserGameByIdAsync(int id)
        {
            return _userGameRepository.GetByIdAsync(id);
        }

        public Task<UserGame> CreateUserGameAsync(UserGame userGame)
        {
            return _userGameRepository.AddAsync(userGame);
        }

        public Task<bool> UpdateUserGameAsync(UserGame userGame)
        {
            return _userGameRepository.UpdateAsync(userGame);
        }

        public Task<bool> DeleteUserGameAsync(int id)
        {
            return _userGameRepository.DeleteAsync(id);
        }

        // Методи роботи з прогресом
        public async Task<bool> AddPlaytimeAsync(int userGameId, int hours)
        {
            var userGame = await _userGameRepository.GetByIdAsync(userGameId);
            if (userGame == null) return false;

            userGame.Hours += hours;
            return await _userGameRepository.UpdateAsync(userGame);
        }

        public async Task<bool> UpdateProgressAsync(int userGameId, int progress)
        {
            var userGame = await _userGameRepository.GetByIdAsync(userGameId);
            if (userGame == null) return false;

            userGame.Progress = progress;
            return await _userGameRepository.UpdateAsync(userGame);
        }

        public async Task<bool> MarkAsCompletedAsync(int userGameId)
        {
            var userGame = await _userGameRepository.GetByIdAsync(userGameId);
            if (userGame == null) return false;

            userGame.Status = "completed";
            userGame.Progress = 100;
            return await _userGameRepository.UpdateAsync(userGame);
        }

        // Статистика
        public Task<int> GetTotalPlaytimeAsync(string userNickname)
        {
            // Потрібно додати в репозиторії метод GetByNickname
            return Task.FromResult(0);
        }

        public async Task<List<UserGame>> GetTopGamesByPlaytimeAsync(string userNickname, int count)
        {
            return new List<UserGame>();
        }

        public Task<List<UserGame>> GetWishlistAsync(string userNickname)
        {
            return Task.FromResult(new List<UserGame>());
        }

        public Task<List<UserGame>> GetActiveGamesAsync(string userNickname)
        {
            return Task.FromResult(new List<UserGame>());
        }

        public Task<List<UserGame>> GetCompletedGamesAsync(string userNickname)
        {
            return Task.FromResult(new List<UserGame>());
        }
    }
}