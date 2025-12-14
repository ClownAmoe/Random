// /BLL/Extensions/CommentExtensions.cs

using System;
using GameOverDose.DAL.Entities; // Посилаємося на DAL

namespace GameOverDose.BLL.Extensions;

public static class CommentExtensions
{
    /// <summary>
    /// Перевіряє чи коментар є позитивним (оцінка >= 7)
    /// </summary>
    public static bool IsPositive(this Comment comment)
    {
        return comment.Rating >= 7;
    }

    /// <summary>
    /// Отримує категорію оцінки
    /// </summary>
    public static string GetRatingCategory(this Comment comment)
    {
        if (comment.Rating == null)
            return "Без оцінки";

        return comment.Rating switch
        {
            >= 9 => "Шедевр",
            >= 7 => "Відмінно",
            // ...
            _ => "Погано"
        };
    }

    /// <summary>
    /// Отримує емодзі для оцінки
    /// </summary>
    public static string GetRatingEmoji(this Comment comment)
    {
        if (comment.Rating == null)
            return "❓";

        return comment.Rating switch
        {
            >= 9 => "🏆",
            >= 7 => "😊",
            // ...
            _ => "😞"
        };
    }

    // ... додайте інші логічні методи (IsNew, GetShortText) сюди.
}