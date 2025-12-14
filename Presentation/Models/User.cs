// Файл: Models/User.cs

using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.Models
{
    // Робимо Observable, якщо дані профілю можуть змінюватися в UI
    public partial class User : ObservableObject
    {
        [ObservableProperty]
        private string username = string.Empty; // ✅ Виправлення CS8618

        [ObservableProperty]
        private string email = string.Empty;    // ✅ Виправлення CS8618

        // Тут можна додати інші поля: дата реєстрації, аватар тощо.
    }
}