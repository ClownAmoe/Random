// /BLL/Extensions/FriendExtensions.cs

using System;
using GameOverDose.DAL.Entities; // Посилаємося на DAL

namespace GameOverDose.BLL.Extensions;

public static class FriendExtensions
{
    /// <summary>
    /// Перевіряє чи дружба очікує підтвердження
    /// </summary>
    public static bool IsPending(this Friend friendship)
    {
        return friendship.Status?.Equals("pending", StringComparison.OrdinalIgnoreCase) ?? false;
    }

    // Метод: Отримує текстовий опис тривалості дружби
    public static string GetFriendshipDurationText(this Friend friendship)
    {
        var duration = DateTime.Now - friendship.CreatedAt;

        if (duration.TotalDays < 1)
            return "Менше дня";
        // ... (решта логіки розрахунку)

        return $"{(int)(duration.TotalDays / 365)} років";
    }

    // ... методи Accept(), Block(), Reject() можна перенести в BLL як методи сервісу (наприклад, FriendService).
}