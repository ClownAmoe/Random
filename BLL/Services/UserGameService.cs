using GameOverDose.BLL.Interfaces;
using GameOverDose.DAL.Entities;
using GameOverDose.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GameOverDose.BLL.Services
{
    public class UserGameService : IUserGameService
    {
        private readonly IUserGameRepository _userGameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;

        public UserGameService(
            IUserGameRepository userGameRepository,
            IUserRepository userRepository,
            IGameRepository gameRepository)
        {
            _userGameRepository = userGameRepository;
            _userRepository = userRepository;
            _gameRepository = gameRepository;
        }

        public async Task<List<UserGame>> GetUserGamesAsync(string userNickname)
        {
            var user = await _userRepository.GetByNicknameAsync(userNickname);
            if (user == null) return new List<UserGame>();

            return await _userGameRepository.GetByUserIdAsync(user.Id);
        }

        public async Task<List<UserGame>> GetGamePlayersAsync(string gameSlug)
        {
            var game = await _gameRepository.GetBySlugAsync(gameSlug);
            if (game == null) return new List<UserGame>();

            return await _userGameRepository.GetByGameIdAsync(game.Id);
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

        public async Task<int> GetTotalPlaytimeAsync(string userNickname)
        {
            var user = await _userRepository.GetByNicknameAsync(userNickname);
            if (user == null) return 0;

            return await _userGameRepository.GetTotalPlaytimeByUserAsync(user.Id);
        }

        public async Task<List<UserGame>> GetTopGamesByPlaytimeAsync(string userNickname, int count)
        {
            var user = await _userRepository.GetByNicknameAsync(userNickname);
            if (user == null) return new List<UserGame>();

            return await _userGameRepository.GetTopGamesByPlaytimeAsync(user.Id, count);
        }

        public async Task<List<UserGame>> GetWishlistAsync(string userNickname)
        {
            var user = await _userRepository.GetByNicknameAsync(userNickname);
            if (user == null) return new List<UserGame>();

            return await _userGameRepository.GetByStatusAsync(user.Id, "wishlist");
        }

        public async Task<List<UserGame>> GetActiveGamesAsync(string userNickname)
        {
            var user = await _userRepository.GetByNicknameAsync(userNickname);
            if (user == null) return new List<UserGame>();

            return await _userGameRepository.GetByStatusAsync(user.Id, "playing");
        }

        public async Task<List<UserGame>> GetCompletedGamesAsync(string userNickname)
        {
            var user = await _userRepository.GetByNicknameAsync(userNickname);
            if (user == null) return new List<UserGame>();

            return await _userGameRepository.GetByStatusAsync(user.Id, "completed");
        }

        public async Task<bool> UpdateTrackingStatusAsync(int userId, int gameId, bool isTracking)
        {
            return await _userGameRepository.UpdateTrackingStatusAsync(userId, gameId, isTracking);
        }

        public async Task<bool> GetTrackingStatusAsync(int userId, int gameId)
        {
            var userGame = await _userGameRepository.GetByUserAndGameAsync(userId, gameId);
            return userGame?.IsTracking ?? false;
        }
    }
}