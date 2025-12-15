using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GameOverDose.DAL.Entities
{
    [Table("games")]
    public class Game
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("slug")]
        public string Slug { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("release")]
        public DateTime? Release { get; set; }

        [Column("description_text")]
        public string Description { get; set; } = string.Empty;

        [Column("tba")]
        public bool Tba { get; set; } = false;

        [MaxLength(500)]
        [Column("background_image")]
        public string BackgroundImage { get; set; } = string.Empty;

        [Column("rating")]
        public double? Rating { get; set; }

        [Column("rating_top")]
        public int? RatingTop { get; set; }

        [MaxLength(50)]
        [Column("ratings")]
        public string Ratings { get; set; } = string.Empty;

        [Column("ratings_top")]
        public int? RatingsTop { get; set; }

        [Column("ratings_count")]
        public int? RatingsCount { get; set; }

        [Column("rewiews_text_count")]
        public int? RewiewsTextCount { get; set; }

        [Column("added")]
        public int? Added { get; set; }

        [MaxLength(100)]
        [Column("added_by_status")]
        public string AddedByStatus { get; set; } = string.Empty;

        [Column("metacritics")]
        public int? Metacritics { get; set; }

        [Column("playtime")]
        public int? Playtime { get; set; }

        [Column("suggestions_count")]
        public int? SuggestionsCount { get; set; }

        [Column("updated")]
        public DateTime? Updated { get; set; }

        [MaxLength(50)]
        [Column("esrb_rating")]
        public string EsrbRating { get; set; } = string.Empty;

        [Column("platforms")]
        public string Platforms { get; set; } = string.Empty;

        [Column("price")]
        public decimal? Price { get; set; }

        [MaxLength(500)]
        [Column("trailer_url")]
        public string? TrailerUrl { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<UserGame> UserGames { get; set; }

        public Game()
        {
            Comments = new HashSet<Comment>();
            UserGames = new HashSet<UserGame>();
        }

        public override string ToString()
        {
            return $"{Name} ({Release?.Year ?? 0}) - {Rating:F1}★";
        }

        public double GetPositiveRatingPercentage()
        {
            if (Rating == null || RatingTop == null || RatingTop == 0)
                return 0;

            return Rating.Value / (double)RatingTop.Value * 100;
        }

        public bool IsNewRelease()
        {
            if (Release == null)
                return false;

            return Release.Value > DateTime.Now.AddYears(-1);
        }

        public string GetPlatformsShort()
        {
            if (string.IsNullOrEmpty(Platforms))
                return "N/A";

            var platforms = Platforms.Split(',').Select(p => p.Trim());

            return platforms.Count() > 3
                ? $"{string.Join(", ", platforms.Take(3))}..."
                : Platforms;
        }
    }
}