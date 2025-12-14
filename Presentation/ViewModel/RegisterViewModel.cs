// Файл: ViewModels/RegisterViewModel.cs

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Services;
using System.Windows; // Для демонстрації повідомлень

namespace Presentation.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        // **********************************************
        // 1. ВЛАСТИВОСТІ ВВЕДЕННЯ ДАНИХ
        // **********************************************

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string email;

        // Примітка: для паролів у WPF у чистому MVVM краще використовувати 
        // PasswordBox з прив'язкою через Attached Property, але тут 
        // ми використовуємо звичайний string для простоти прикладу.
        [ObservableProperty]
        private string password;

        // **********************************************
        // 2. КОНСТРУКТОР
        // **********************************************

        public RegisterViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        // **********************************************
        // 3. КОМАНДИ
        // **********************************************

        // Команда для обробки процесу реєстрації
        [RelayCommand]
        private void Register()
        {
            // 🛑 У РЕАЛЬНОМУ ДОДАТКУ:
            // 1. Валідація полів (чи не порожні, чи коректний email, чи сильний пароль).
            // 2. Виклик сервісу аутентифікації (AuthService.Register(Username, Email, Password)).

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Будь ласка, введіть ім'я користувача та пароль.", "Помилка реєстрації");
                return;
            }

            // Якщо реєстрація успішна, переходимо на сторінку логіну (або одразу на головну)
            MessageBox.Show($"Користувач {Username} успішно зареєстрований!", "Успіх");

            // Перехід на сторінку Логіну
            _navigationService.NavigateTo<LoginViewModel>();
        }

        // Команда для повернення на сторінку логіну
        [RelayCommand]
        private void GoToLogin()
        {
            _navigationService.NavigateTo<LoginViewModel>();
        }
    }
}