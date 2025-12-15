using GameOverDose.DAL.Entities;
using GameOverDose.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOverDose.DAL.Repositories
{
    public class UserGameRepository : IUserGameRepository
    {
        private readonly GameOverDoseDbContext _context;

        public UserGameRepository(GameOverDoseDbContext context)
        {
            _context = context;
        }

        public async Task<UserGame?> GetByIdAsync(int id)
        {
            return await _context.UserGames
                .Include(ug => ug.User)
                .Include(ug => ug.Game)
                .FirstOrDefaultAsync(ug => ug.Id == id);
        }

        public async Task<List<UserGame>> GetAllAsync()
        {
            return await _context.UserGames
                .Include(ug => ug.User)
                .Include(ug => ug.Game)
                .ToListAsync();
        }

        public async Task<UserGame> AddAsync(UserGame userGame)
        {
            await _context.UserGames.AddAsync(userGame);
            await _context.SaveChangesAsync();
            return userGame;
        }

        public async Task<bool> UpdateAsync(UserGame userGame)
        {
            try
            {
                _context.UserGames.Update(userGame);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var userGame = await _context.UserGames.FindAsync(id);
            if (userGame == null) return false;

            _context.UserGames.Remove(userGame);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserGame>> GetByUserIdAsync(int userId)
        {
            return await _context.UserGames
                .Include(ug => ug.Game)
                .Where(ug => ug.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<UserGame>> GetByGameIdAsync(int gameId)
        {
            return await _context.UserGames
                .Include(ug => ug.User)
                .Where(ug => ug.GameId == gameId)
                .ToListAsync();
        }

        public async Task<UserGame?> GetByUserAndGameAsync(int userId, int gameId)
        {
            return await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId);
        }

        public async Task<int> GetTotalPlaytimeByUserAsync(int userId)
        {
            return await _context.UserGames
                .Where(ug => ug.UserId == userId)
                .SumAsync(ug => ug.Hours);
        }

        public Task<List<UserGame>> GetTopGamesByPlaytimeAsync(int userId, int count)
        {
            return Task.FromResult(_context.UserGames
                .Include(ug => ug.Game)
                .Where(ug => ug.UserId == userId)
                .OrderByDescending(ug => ug.Hours)
                .Take(count)
                .ToList());
        }

        public Task<List<UserGame>> GetByStatusAsync(int userId, string status)
        {
            return Task.FromResult(_context.UserGames
                .Include(ug => ug.Game)
                .Where(ug => ug.UserId == userId && ug.Status == status)
                .ToList());
        }

        public async Task<bool> UpdateTrackingStatusAsync(int userId, int gameId, bool isTracking)
        {
            var userGame = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId);

            if (userGame == null)
            {
                userGame = new UserGame
                {
                    UserId = userId,
                    GameId = gameId,
                    IsTracking = isTracking,
                    Status = "playing",
                    AddedAt = DateTime.Now
                };
                await _context.UserGames.AddAsync(userGame);
            }
            else
            {
                userGame.IsTracking = isTracking;
                _context.UserGames.Update(userGame);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
