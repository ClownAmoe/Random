// BLL/Services/CommentService.cs

using GameOverDose.BLL.Interfaces;
using GameOverDose.DAL.Entities;
using GameOverDose.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace GameOverDose.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        // ========================================
        // CRUD
        // ========================================

        public Task<List<Comment>> GetAllCommentsAsync()
        {
            return _commentRepository.GetAllAsync();
        }

        public Task<Comment?> GetCommentByIdAsync(int id)
        {
            return _commentRepository.GetByIdAsync(id);
        }

        public Task<List<Comment>> GetCommentsByGameAsync(int gameId)
        {
            return _commentRepository.GetByGameIdAsync(gameId);
        }

        public Task<List<Comment>> GetCommentsByUserAsync(int userId)
        {
            return _commentRepository.GetByUserIdAsync(userId);
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            // Бізнес-логіка валідації
            if (string.IsNullOrWhiteSpace(comment.Text) && !comment.Rating.HasValue)
            {
                throw new ArgumentException("Коментар повинен містити текст або оцінку");
            }

            if (comment.Rating.HasValue && (comment.Rating < 1 || comment.Rating > 10))
            {
                throw new ArgumentException("Оцінка повинна бути від 1 до 10");
            }

            comment.CreatedAt = DateTime.Now;
            return await _commentRepository.AddAsync(comment);
        }

        public Task<bool> UpdateCommentAsync(Comment comment)
        {
            return _commentRepository.UpdateAsync(comment);
        }

        public Task<bool> DeleteCommentAsync(int id)
        {
            return _commentRepository.DeleteAsync(id);
        }

        // ========================================
        // Специфічні методи
        // ========================================

        public Task<List<Comment>> GetRecentCommentsAsync(int count)
        {
            return _commentRepository.GetRecentAsync(count);
        }

        public Task<List<Comment>> GetPositiveCommentsAsync()
        {
            return _commentRepository.GetByRatingRangeAsync(7, 10);
        }

        public Task<List<Comment>> GetNegativeCommentsAsync()
        {
            return _commentRepository.GetByRatingRangeAsync(1, 4);
        }

        public Task<double?> GetAverageRatingForGameAsync(int gameId)
        {
            return _commentRepository.GetAverageRatingByGameIdAsync(gameId);
        }

        public Task<int> GetCommentCountForGameAsync(int gameId)
        {
            return _commentRepository.GetCountByGameIdAsync(gameId);
        }

        public Task<bool> HasUserCommentedGameAsync(int userId, int gameId)
        {
            return _commentRepository.HasUserCommentedGameAsync(userId, gameId);
        }
    }
}