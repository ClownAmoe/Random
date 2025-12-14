// /BLL/Extensions/UserGameExtensions.cs

using System;
using GameOverDose.DAL.Entities; // Ми посилаємося на DAL!

namespace GameOverDose.BLL.Extensions;

public static class UserGameExtensions
{
    // Метод, перенесений з DAL
    public static bool IsActivelyPlaying(this UserGame ug)
    {
        // Логіка, яка була в DAL, тепер тут.
        return ug.LastPlayed.HasValue &&
               ug.LastPlayed.Value > DateTime.Now.AddDays(-7) &&
               ug.Status == "playing";
    }

    // Метод, перенесений з DAL
    public static string GetStatusText(this UserGame ug)
    {
        return ug.Status switch
        {
            "wishlist" => "В списку бажань",
            "playing" => "Граю зараз",
            // ...
            _ => "Невідомо"
        };
    }

    // ... інші методи логіки (AddHours, MarkAsCompleted, GetExperienceLevel)
}