// /DAL/Repositories/GameRepository.cs

using GameOverDose.DAL.Entities;
using GameOverDose.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOverDose.DAL.Repositories
{
    // ПРИПУЩЕННЯ: Використовуйте фактичну назву вашого DbContext
    public class GameRepository : IGameRepository
    {
        private readonly GameOverDoseDbContext _context;

        // Конструктор: залежність від контексту бази даних
        public GameRepository(GameOverDoseDbContext context)
        {
            _context = context;
        }

        // ======================================
        // CRUD-методи
        // ======================================

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task<List<Game>> GetAllAsync()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<Game> AddAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<bool> UpdateAsync(Game game)
        {
            try
            {
                // Припускаємо, що об'єкт 'game' вже відстежується або має бути оновлений
                _context.Games.Update(game);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Обробка, якщо запис не знайдено або виник конфлікт
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return false;

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }

        // ======================================
        // Специфічні запити
        // ======================================

        public async Task<Game?> GetBySlugAsync(string slug)
        {
            return await _context.Games
                                 .FirstOrDefaultAsync(g => g.Slug == slug);
        }

        public async Task<bool> ExistsBySlugAsync(string slug)
        {
            return await _context.Games
                                 .AnyAsync(g => g.Slug == slug);
        }

        public async Task<List<Game>> SearchAsync(string searchText)
        {
            // Пошук за назвою або іншими полями
            return await _context.Games
                                 .Where(g => g.Name.Contains(searchText))
                                 .ToListAsync();
        }

        public async Task<List<Game>> GetTopRatedAsync(int count)
        {
            // Вибираємо ігри з найвищим рейтингом
            return await _context.Games
                                 .OrderByDescending(g => g.Rating)
                                 .Take(count)
                                 .ToListAsync();
        }

        public async Task<List<Game>> GetNewReleasesAsync()
        {
            // Вибираємо ігри, які вийшли нещодавно
            var lastYear = DateTime.Now.AddYears(-1);
            return await _context.Games
                                 .Where(g => g.Release.HasValue && g.Release.Value >= lastYear)
                                 .OrderByDescending(g => g.Release)
                                 .ToListAsync();
        }

        public async Task<List<Game>> GetByPlatformAsync(string platform)
        {
            // Шукаємо ігри за назвою платформи (припускаючи, що Platforms зберігається як рядок з роздільниками)
            // Примітка: Для більш надійного пошуку потрібна нормалізована таблиця Platform
            return await _context.Games
                                 .Where(g => g.Platforms.Contains(platform))
                                 .ToListAsync();
        }

        public async Task<Game?> GetWithCommentsAsync(int id)
        {
            // Завантажуємо гру разом із коментарями
            return await _context.Games
                                 .Include(g => g.Comments)
                                 .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Game?> GetGameWithCommentsAndUsersAsync(int id)
        {
            // Завантажуємо гру, коментарі та користувачів, які їх залишили
            return await _context.Games
                                 .Include(g => g.Comments)
                                    .ThenInclude(c => c.User) // Завантажуємо User для кожного Comment
                                 .FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}