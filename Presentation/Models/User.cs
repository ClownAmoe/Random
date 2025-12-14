// Файл: Models/User.cs

using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.Models
{
    // Робимо Observable, якщо дані профілю можуть змінюватися в UI
    public partial class User : ObservableObject
    {
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string email;

        // Тут можна додати інші поля: дата реєстрації, аватар тощо.
    }
}