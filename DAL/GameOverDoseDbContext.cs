// /DAL/GameOverDoseDbContext.cs

using Microsoft.EntityFrameworkCore;
using GameOverDose.DAL.Entities;

namespace GameOverDose.DAL;

public class GameOverDoseDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<UserGame> UserGames { get; set; }
    public DbSet<Friend> Friends { get; set; }

    public GameOverDoseDbContext(DbContextOptions<GameOverDoseDbContext> options) : base(options)
    {
        // Конструктор для DI
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ========================================
        // 1. Налаштування Зв'язку Дружби (самопосилання)
        // ========================================

        modelBuilder.Entity<Friend>()
            .HasOne(f => f.User1)
            .WithMany(u => u.FriendsAsUser1)
            .HasForeignKey(f => f.UserId1)
            .OnDelete(DeleteBehavior.Restrict); // Запобігає циклічним або каскадним видаленням

        modelBuilder.Entity<Friend>()
            .HasOne(f => f.User2)
            .WithMany(u => u.FriendsAsUser2)
            .HasForeignKey(f => f.UserId2)
            .OnDelete(DeleteBehavior.Restrict);

        // ========================================
        // 2. Налаштування Composite Key для UserGame (якщо потрібно)
        // ========================================
        // Якщо ви хочете, щоб не було двох однакових User/Game записів (Id потрібен лише для EF Core)
        // modelBuilder.Entity<UserGame>().HasKey(ug => new { ug.UserId, ug.GameId }); 
    }
}