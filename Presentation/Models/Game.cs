#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace Presentation.Models
{
    public partial class GameModel : ObservableObject
    {
        // Властивості для мапінгу з DAL
        [ObservableProperty]
        private int id;

        // Name використовується для Slug/внутрішньої назви
        [ObservableProperty]
        private string name = string.Empty;

        // Title використовується для відображення в UI
        [ObservableProperty]
        private string title = string.Empty;

        // Час гри
        [ObservableProperty]
        private int hoursPlayed; // Змінено на int, відповідно до IUserGameService

        [ObservableProperty]
        private string imageSource = string.Empty;

        [ObservableProperty]
        private string genre = string.Empty;

        [ObservableProperty]
        private decimal price;

        // **********************************************
        // НОВІ ВЛАСТИВОСТІ ДЛЯ СТОРІНКИ ДЕТАЛЕЙ ГРИ
        // **********************************************

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private List<string> developers = new List<string>();

        [ObservableProperty]
        private string trailerUrl = string.Empty;

        [ObservableProperty]
        private DateTime releaseDate;

        [ObservableProperty]
        private string status = string.Empty; // Наприклад: playing, completed, wishlist
    }
}