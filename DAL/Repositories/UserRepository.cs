using Microsoft.EntityFrameworkCore;
using GameOverDose.DAL.Entities;
using GameOverDose.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameOverDose.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly GameOverDoseDbContext _context;

    public UserRepository(GameOverDoseDbContext context)
    {
        _context = context;
    }

    // Примітка: Більшість методів CRUD для DAL можна уніфікувати в GenericRepository
    // Але для ясності, реалізуємо їх тут

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        try
        {
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
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    // Специфічні методи
    public async Task<User?> GetByNicknameAsync(string nickname)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);
    }

    public async Task<bool> ExistsByNicknameAsync(string nickname)
    {
        return await _context.Users.AnyAsync(u => u.Nickname == nickname);
    }

    public async Task<List<User>> SearchAsync(string searchText)
    {
        var search = searchText.ToLower();
        return await _context.Users
            .Where(u => u.Nickname.ToLower().Contains(search) || u.Email.ToLower().Contains(search))
            .ToListAsync();
    }

    public async Task<List<User>> GetTopByLevelAsync(int count)
    {
        return await _context.Users
            .OrderByDescending(u => u.Lvl)
            .Take(count)
            .ToListAsync();
    }

    public async Task<User?> GetWithGamesAsync(int id)
    {
        // Включаємо навігаційні властивості
        return await _context.Users
            .Include(u => u.UserGames)
                .ThenInclude(ug => ug.Game)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetWithCommentsAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Comments)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}