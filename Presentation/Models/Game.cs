// Файл: Presentation/Models/Game.cs (ВИПРАВЛЕНО)

#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace Presentation.Models
{
    // Клас має бути partial і успадковуватись від ObservableObject
    public partial class Game : ObservableObject
    {
        // ----------------------------------------------------
        // ✅ ВИПРАВЛЕНО: Додано ініціалізацію = string.Empty;
        // ----------------------------------------------------

        // Властивості для мапінгу з DAL (Title/Name, Price)
        [ObservableProperty]
        private int id;

        // Поле 'name' потрібне для мапінгу з DAL, 
        // але в DTO для UI ви можете використовувати 'title'
        [ObservableProperty]
        private string name = string.Empty;

        // Існуючі властивості
        [ObservableProperty]
        private string title = string.Empty; // ✅ Ініціалізація

        [ObservableProperty]
        private string imageSource = string.Empty; // ✅ Ініціалізація

        [ObservableProperty]
        private string genre = string.Empty; // ✅ Ініціалізація

        [ObservableProperty]
        private decimal price; // decimal є value type, не потребує ініціалізації

        // **********************************************
        // НОВІ ВЛАСТИВОСТІ ДЛЯ СТОРІНКИ ДЕТАЛЕЙ ГРИ
        // **********************************************

        [ObservableProperty]
        private string description = string.Empty; // ✅ Ініціалізація

        // ✅ Ініціалізація List<string> новим об'єктом
        [ObservableProperty]
        private List<string> developers = new List<string>();

        [ObservableProperty]
        private string trailerUrl = string.Empty; // ✅ Ініціалізація

        [ObservableProperty]
        private DateTime releaseDate; // DateTime є value type, не потребує ініціалізації
    }
}