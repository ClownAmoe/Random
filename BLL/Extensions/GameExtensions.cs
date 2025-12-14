// /BLL/Extensions/GameExtensions.cs

using System;
using System.Linq;
using GameOverDose.DAL.Entities; // Посилаємося на DAL

namespace GameOverDose.BLL.Extensions;

public static class GameExtensions
{
    /// <summary>
    /// Розраховує відсоток позитивних оцінок
    /// </summary>
    public static double GetPositiveRatingPercentage(this Game game)
    {
        // Перевірка на null та нульовий RatingTop
        if (game.Rating == null || game.RatingTop == null || game.RatingTop == 0)
            return 0;

        // ВИПРАВЛЕНО: Використовуємо .Value для безпечного доступу до double/int
        return game.Rating.Value / (double)game.RatingTop.Value * 100;
    }

    /// <summary>
    /// Перевіряє чи гра є новою (вийшла менше року тому)
    /// </summary>
    public static bool IsNewRelease(this Game game)
    {
        if (game.Release == null)
            return false;

        return game.Release.Value > DateTime.Now.AddYears(-1);
    }

    /// <summary>
    /// Отримує короткий опис платформ
    /// </summary>
    public static string GetPlatformsShort(this Game game)
    {
        if (string.IsNullOrEmpty(game.Platforms))
            return "N/A";

        var platforms = game.Platforms.Split(',');

        // Логіка скорочення тексту
        return platforms.Length > 3
            ? $"{string.Join(", ", platforms.Take(3))}..."
            : game.Platforms;
    }
}