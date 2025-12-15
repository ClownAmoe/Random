// Файл: DAL/GameOverDoseDbContext.cs (Оптимізовано для PostgreSQL)

using Microsoft.EntityFrameworkCore;
using GameOverDose.DAL.Entities;
using System;

namespace GameOverDose.DAL
{
    public class GameOverDoseDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<Friend> Friends { get; set; }

        public GameOverDoseDbContext(DbContextOptions<GameOverDoseDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========================================
            // 1. НАЛАШТУВАННЯ ТАБЛИЦІ КОРИСТУВАЧІВ
            // ========================================
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(u => u.Email)
                    .IsUnique()
                    .HasDatabaseName("idx_users_email");

                entity.HasIndex(u => u.Nickname)
                    .IsUnique()
                    .HasDatabaseName("idx_users_nickname");

                // Налаштування каскадного видалення для зв'язків
                entity.HasMany(u => u.Comments)
                    .WithOne(c => c.User)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.UserGames)
                    .WithOne(ug => ug.User)
                    .HasForeignKey(ug => ug.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ========================================
            // 2. НАЛАШТУВАННЯ ТАБЛИЦІ ІГОР
            // ========================================
            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("games");

                entity.HasIndex(g => g.Slug)
                    .IsUnique()
                    .HasDatabaseName("idx_games_slug");

                entity.HasIndex(g => g.Rating)
                    .HasDatabaseName("idx_games_rating");

                entity.Property(g => g.Description)
                    .HasColumnName("description_text");

                // Налаштування точності для decimal
                entity.Property(g => g.Price)
                    .HasColumnType("decimal(10,2)");

                // Каскадне видалення
                entity.HasMany(g => g.Comments)
                    .WithOne(c => c.Game)
                    .HasForeignKey(c => c.GameId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(g => g.UserGames)
                    .WithOne(ug => ug.Game)
                    .HasForeignKey(ug => ug.GameId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ========================================
            // 3. НАЛАШТУВАННЯ ТАБЛИЦІ КОМЕНТАРІВ
            // ========================================
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                entity.HasIndex(c => c.UserId)
                    .HasDatabaseName("idx_comment_userid");

                entity.HasIndex(c => c.GameId)
                    .HasDatabaseName("idx_comment_gameid");

                // Значення за замовчуванням для Created_At
                entity.Property(c => c.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // ========================================
            // 4. НАЛАШТУВАННЯ ТАБЛИЦІ ІГРОВИХ СЕСІЙ
            // ========================================
            modelBuilder.Entity<UserGame>(entity =>
            {
                entity.ToTable("usergame");

                // Унікальний індекс на комбінацію UserId + GameId
                entity.HasIndex(ug => new { ug.UserId, ug.GameId })
                    .IsUnique()
                    .HasDatabaseName("idx_usergame_user_game");

                entity.HasIndex(ug => ug.UserId)
                    .HasDatabaseName("idx_usergame_userid");

                entity.HasIndex(ug => ug.GameId)
                    .HasDatabaseName("idx_usergame_gameid");

                // Значення за замовчуванням
                entity.Property(ug => ug.AddedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // ========================================
            // 5. НАЛАШТУВАННЯ ТАБЛИЦІ ДРУЖБИ (САМОПОСИЛАННЯ)
            // ========================================
            modelBuilder.Entity<Friend>(entity =>
            {
                entity.ToTable("friend");

                // Відносини з User1
                entity.HasOne(f => f.User1)
                    .WithMany(u => u.FriendsAsUser1)
                    .HasForeignKey(f => f.UserId1)
                    .OnDelete(DeleteBehavior.Restrict); // Запобігає циклічному видаленню

                // Відносини з User2
                entity.HasOne(f => f.User2)
                    .WithMany(u => u.FriendsAsUser2)
                    .HasForeignKey(f => f.UserId2)
                    .OnDelete(DeleteBehavior.Restrict);

                // Унікальний індекс на комбінацію UserId1 + UserId2
                entity.HasIndex(f => new { f.UserId1, f.UserId2 })
                    .IsUnique()
                    .HasDatabaseName("idx_friend_users");

                entity.HasIndex(f => f.UserId1)
                    .HasDatabaseName("idx_friend_userid1");

                entity.HasIndex(f => f.UserId2)
                    .HasDatabaseName("idx_friend_userid2");

                // Значення за замовчуванням
                entity.Property(f => f.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Обмеження: користувач не може бути другом сам собі
                entity.HasCheckConstraint("CK_Friend_DifferentUsers", "userid1 <> userid2");
            });

            // ========================================
            // 6. НАЛАШТУВАННЯ КОНВЕРТАЦІЇ ДЛЯ POSTGRESQL
            // ========================================

            // PostgreSQL не підтримує DateTime2, використовуємо timestamp
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("timestamp without time zone");
                    }
                }
            }

            // ========================================
            // 7. НАЛАШТУВАННЯ НАЗИВАННЯ ДЛЯ POSTGRESQL
            // ========================================

            // PostgreSQL чутлива до регістру, тому використовуємо lowercase
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Назви таблиць та колонок вже налаштовано через атрибути [Table] та [Column]
                // Але для забезпечення сумісності можна додатково налаштувати:

                foreach (var property in entity.GetProperties())
                {
                    // Переконуємося, що всі колонки мають назви у lowercase
                    if (property.GetColumnName() == null)
                    {
                        property.SetColumnName(property.Name.ToLower());
                    }
                }
            }
        }

        // ========================================
        // МЕТОДИ ДЛЯ НАЛАГОДЖЕННЯ
        // ========================================

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Цей метод викличеться, тільки якщо DbContext не налаштовано через DI
                // Корисно для міграцій
                optionsBuilder.UseNpgsql("Host=localhost;Database=GameOverDoseDB;Username=postgres;Password=123123xdd");
            }

            // Увімкнути детальне логування (тільки для розробки)
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
#endif
        }
    }
}