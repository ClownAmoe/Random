using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq; // Додано для методу GetPlatformsShort
using GameOverDose.DAL.Entities; // Хоча знаходиться в цьому ж просторі імен, іноді корисно

namespace GameOverDose.DAL.Entities
{
    /// <summary>
    /// Модель відеогри
    /// </summary>
    [Table("games")]
    public class Game
    {
        /// <summary>
        /// Унікальний ідентифікатор гри
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Slug - унікальний URL-friendly ідентифікатор
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Column("slug")]
        public string Slug { get; set; } = string.Empty; // <-- ВИПРАВЛЕНО (CS8618)

        /// <summary>
        /// Повна назва гри
        /// </summary>
        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty; // <-- ВИПРАВЛЕНО (CS8618)

        /// <summary>
        /// Дата релізу гри
        /// </summary>
        [Column("release")]
        public DateTime? Release { get; set; }

        [Column("release")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// To Be Announced - чи дата релізу ще не оголошена
        /// </summary>
        [Column("tba")]
        public bool Tba { get; set; } = false;

        /// <summary>
        /// URL фонового зображення гри
        /// </summary>
        [MaxLength(255)]
        [Column("background_image")]
        public string BackgroundImage { get; set; } = string.Empty; // <-- ВИПРАВЛЕНО (CS8618)

        /// <summary>
        /// Середній рейтинг гри (0-5)
        /// </summary>
        [Column("rating")]
        public double? Rating { get; set; }

        /// <summary>
        /// Найвищий можливий рейтинг
        /// </summary>
        [Column("rating_top")]
        public int? RatingTop { get; set; }

        /// <summary>
        /// Категорія рейтингів (наприклад: "exceptional", "recommended")
        /// </summary>
        [MaxLength(50)]
        [Column("ratings")]
        public string Ratings { get; set; } = string.Empty; // <-- ВИПРАВЛЕНО (CS8618)

        /// <summary>
        /// Найбільший рейтинг серед всіх оцінок
        /// </summary>
        [Column("ratings_top")]
        public int? RatingsTop { get; set; }

        /// <summary>
        /// Загальна кількість оцінок
        /// </summary>
        [Column("ratings_count")]
        public int? RatingsCount { get; set; }

        /// <summary>
        /// Кількість текстових відгуків
        /// </summary>
        [Column("rewiews_text_count")]
        public int? RewiewsTextCount { get; set; }

        /// <summary>
        /// Кількість разів додана до бібліотек користувачів
        /// </summary>
        [Column("added")]
        public int? Added { get; set; }

        /// <summary>
        /// Статистика додавання по статусам
        /// </summary>
        [MaxLength(100)]
        [Column("added_by_status")]
        public string AddedByStatus { get; set; } = string.Empty; // <-- ВИПРАВЛЕНО (CS8618)

        /// <summary>
        /// Оцінка Metacritic (0-100)
        /// </summary>
        [Column("metacritics")]
        public int? Metacritics { get; set; }

        /// <summary>
        /// Середній час проходження гри (в годинах)
        /// </summary>
        [Column("playtime")]
        public int? Playtime { get; set; }

        /// <summary>
        /// Кількість рекомендацій
        /// </summary>
        [Column("suggestions_count")]
        public int? SuggestionsCount { get; set; }

        /// <summary>
        /// Дата останнього оновлення інформації про гру
        /// </summary>
        [Column("updated")]
        public DateTime? Updated { get; set; }

        /// <summary>
        /// ESRB рейтинг (вікове обмеження)
        /// </summary>
        [MaxLength(50)]
        [Column("esrb_rating")]
        public string EsrbRating { get; set; } = string.Empty; // <-- ВИПРАВЛЕНО (CS8618)

        /// <summary>
        /// Список платформ (через кому)
        /// </summary>
        [Column("platforms")]
        public string Platforms { get; set; } = string.Empty; // <-- ВИПРАВЛЕНО (CS8618)

        /// <summary>
        /// Поточна ціна гри (може бути нульовою, якщо безкоштовна)
        /// </summary>
        [Column("price")]
        public decimal? Price { get; set; }

        // ========================================
        // Навігаційні властивості (зв'язки)
        // ========================================

        /// <summary>
        /// Коментарі до цієї гри
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Ігрові сесії користувачів для цієї гри
        /// </summary>
        public virtual ICollection<UserGame> UserGames { get; set; }

        /// <summary>
        /// Конструктор за замовчуванням
        /// </summary>
        public Game()
        {
            Comments = new HashSet<Comment>();
            UserGames = new HashSet<UserGame>();
            // Updated = DateTime.Now; // Залишаємо опціонально
        }

        /// <summary>
        /// Перевизначення ToString для зручного відображення
        /// </summary>
        public override string ToString()
        {
            return $"{Name} ({Release?.Year ?? 0}) - {Rating:F1}★";
        }

        /// <summary>
        /// Розраховує відсоток позитивних оцінок
        /// </summary>
        public double GetPositiveRatingPercentage()
        {
            if (Rating == null || RatingTop == null || RatingTop == 0)
                return 0;

            // RatingTop є int?, тому використовуємо .Value для безпечного доступу після перевірки на null
            return Rating.Value / RatingTop.Value * 100;
        }

        /// <summary>
        /// Перевіряє чи гра є новою (вийшла менше року тому)
        /// </summary>
        public bool IsNewRelease()
        {
            if (Release == null)
                return false;

            return Release.Value > DateTime.Now.AddYears(-1);
        }

        /// <summary>
        /// Отримує короткий опис платформ
        /// </summary>
        public string GetPlatformsShort()
        {
            if (string.IsNullOrEmpty(Platforms))
                return "N/A";

            // Використовуємо System.Linq для Split та Take
            var platforms = Platforms.Split(',').Select(p => p.Trim());

            return platforms.Count() > 3
                ? $"{string.Join(", ", platforms.Take(3))}..."
                : Platforms;
        }
    }
}