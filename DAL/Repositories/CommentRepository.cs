// DAL/Repositories/CommentRepository.cs

using GameOverDose.DAL.Entities;
using GameOverDose.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOverDose.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly GameOverDoseDbContext _context;

        public CommentRepository(GameOverDoseDbContext context)
        {
            _context = context;
        }

        // ========================================
        // CRUD
        // ========================================

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Game)
                .FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Game)
                .ToListAsync();
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> UpdateAsync(Comment comment)
        {
            try
            {
                _context.Comments.Update(comment);
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
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        // ========================================
        // Специфічні запити
        // ========================================

        public async Task<List<Comment>> GetByGameIdAsync(int gameId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.GameId == gameId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetByUserIdAsync(int userId)
        {
            return await _context.Comments
                .Include(c => c.Game)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetRecentAsync(int count)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Game)
                .OrderByDescending(c => c.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetByRatingRangeAsync(int minRating, int maxRating)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Game)
                .Where(c => c.Rating.HasValue && c.Rating >= minRating && c.Rating <= maxRating)
                .ToListAsync();
        }

        // ========================================
        // Агрегація
        // ========================================

        public async Task<double?> GetAverageRatingByGameIdAsync(int gameId)
        {
            var comments = await _context.Comments
                .Where(c => c.GameId == gameId && c.Rating.HasValue)
                .ToListAsync();

            if (!comments.Any()) return null;

            return comments.Average(c => c.Rating!.Value);
        }

        public async Task<int> GetCountByGameIdAsync(int gameId)
        {
            return await _context.Comments
                .Where(c => c.GameId == gameId)
                .CountAsync();
        }

        public async Task<bool> HasUserCommentedGameAsync(int userId, int gameId)
        {
            return await _context.Comments
                .AnyAsync(c => c.UserId == userId && c.GameId == gameId);
        }
    }
}