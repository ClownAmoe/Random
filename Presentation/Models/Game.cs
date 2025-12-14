using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
// Це необхідно для ObservableObject та [ObservableProperty]

namespace Presentation.Models
{
    // Клас має бути partial і успадковуватись від ObservableObject
    public partial class Game : ObservableObject
    {
        // Існуючі властивості
        [ObservableProperty]
        private string title;
        
        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string imageSource;

        [ObservableProperty]
        private string genre;

        [ObservableProperty]
        private decimal price;

        // **********************************************
        // НОВІ ВЛАСТИВОСТІ ДЛЯ СТОРІНКИ ДЕТАЛЕЙ ГРИ
        // **********************************************

        // "smth bout game"
        [ObservableProperty]
        private string description;

        // "Developers: Item One, Item Two..."
        [ObservableProperty]
        private List<string> developers;

        // Посилання на трейлер/геймплей
        [ObservableProperty]
        private string trailerUrl;

        // Додаткові деталі (наприклад, дата випуску)
        [ObservableProperty]
        private DateTime releaseDate;
    }
}