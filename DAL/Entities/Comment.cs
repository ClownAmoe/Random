// /DAL/Entities/Comment.cs
using System;
using System.ComponentModel.DataAnnotations;
using GameOverDose.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOverDose.DAL.Entities;

/// <summary>
/// Сутність: Коментар користувача до гри (чистий DAL)
/// </summary>
[Table("comment")]
public class Comment
{
    [Key]
    [Column("commentid")]
    public int CommentId { get; set; }

    [Required]
    [Column("userid")]
    public int UserId { get; set; }

    [Column("text")]
    public string? Text { get; set; } // Змінено на Nullable, оскільки коментар може бути тільки з оцінкою

    [Range(1, 10)]
    [Column("rating")]
    public int? Rating { get; set; }

    [Required]
    [Column("gameid")]
    public int GameId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ========================================
    // Навігаційні властивості (зв'язки)
    // ========================================

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!; // Припускаємо, що User є сутністю

    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; } = null!; // Припускаємо, що Game є сутністю

    // Метод ToString та логічні методи (IsPositive, GetRatingCategory) видаляємо з DAL
}