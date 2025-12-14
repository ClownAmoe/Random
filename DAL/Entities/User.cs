// /DAL/Entities/User.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOverDose.DAL.Entities;

/// <summary>
/// Сутність: Користувач (чистий DAL)
/// </summary>
[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nickname")]
    public string Nickname { get; set; } = null!; // Додано null!

    [Required]
    [MaxLength(255)]
    [Column("email")]
    public string Email { get; set; } = null!; // Додано null!

    // ПРИМІТКА: Для реального проєкту використовуйте окрему таблицю для хешування паролів або
    // клас PasswordHash (не Entity), щоб уникнути доступу до нього в усіх шарах
    [Required]
    [MaxLength(255)]
    [Column("password")]
    public string Password { get; set; } = null!; // Додано null!

    [MaxLength(255)]
    [Column("avatar")]
    public string? Avatar { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("lvl")]
    public int Lvl { get; set; } = 1;

    // ========================================
    // Навігаційні властивості (зв'язки)
    // ========================================

    // Зворотні зв'язки
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

    public virtual ICollection<UserGame> UserGames { get; set; } = new HashSet<UserGame>();

    // Самопосилальні зв'язки для дружби (потрібно буде налаштувати у DbContext)
    public virtual ICollection<Friend> FriendsAsUser1 { get; set; } = new HashSet<Friend>();

    public virtual ICollection<Friend> FriendsAsUser2 { get; set; } = new HashSet<Friend>();

    // Конструктор
    public User() { }

    // Метод ToString() може залишитися
    public override string ToString()
    {
        return $"{Nickname} (Lvl {Lvl})";
    }
}