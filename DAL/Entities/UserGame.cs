// /DAL/Entities/UserGame.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore; // Потрібно для PrimaryKey

namespace GameOverDose.DAL.Entities;

/// <summary>
/// Сутність: Зв'язок "користувач-гра" (багато-до-багатьох)
/// </summary>
[Table("usergame")]
// Ми використовували PrimaryKey(UserId, GameId) у попередніх планах, 
// але якщо БД вимагає окремий Id, залишаємо його
public class UserGame
{
    [Key]
    [Column("id")]
    public int Id { get; set; } // Унікальний ID запису

    [Required]
    [Column("userid")]
    public int UserId { get; set; } // Змінено на int ID

    [Required]
    [Column("gameid")]
    public int GameId { get; set; } // Змінено на int ID

    [Column("hours")]
    public int Hours { get; set; } = 0;

    [Column("added_at")]
    public DateTime AddedAt { get; set; } = DateTime.Now;

    [Column("last_played")]
    public DateTime? LastPlayed { get; set; }

    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "playing"; // "wishlist", "playing", "completed", etc.

    [Range(0, 100)]
    [Column("progress")]
    public int Progress { get; set; } = 0;

    [Range(1, 10)]
    [Column("personal_rating")]
    public int? PersonalRating { get; set; }

    [Column("is_favorite")]
    public bool IsFavorite { get; set; } = false;

    [Column("is_tracking")]
    public bool IsTracking { get; set; } = false;

    // ========================================
    // Навігаційні властивості (зв'язки)
    // ========================================

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!; // Припускаємо, що User має int Id

    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; } = null!; // Припускаємо, що Game має int Id
}