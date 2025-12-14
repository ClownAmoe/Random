// /DAL/Entities/Friend.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOverDose.DAL.Entities;

/// <summary>
/// Сутність: Зв'язок дружби між користувачами (самопосилання)
/// </summary>
[Table("friend")]
public class Friend
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("userid1")]
    public int UserId1 { get; set; } // Змінено на int

    [Required]
    [Column("userid2")]
    public int UserId2 { get; set; } // Змінено на int

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "accepted"; // "pending", "accepted", "blocked"

    // ========================================
    // Навігаційні властивості (зв'язки)
    // ========================================

    [ForeignKey(nameof(UserId1))]
    public virtual User User1 { get; set; } = null!;

    [ForeignKey(nameof(UserId2))]
    public virtual User User2 { get; set; } = null!;
}