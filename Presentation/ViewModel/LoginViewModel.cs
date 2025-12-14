// Файл: ViewModels/LoginViewModel.cs (ФІНАЛЬНО ВИПРАВЛЕНО)

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Services;
using System.Windows.Controls;
using System.Windows;
using Presentation.ViewModels;

namespace Presentation.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string email = string.Empty;

        // **********************************************
        // КОНСТРУКТОРИ
        // **********************************************

        // ✅ 1. БЕЗПАРАМЕТРИЧНИЙ КОНСТРУКТОР (ДЛЯ ДИЗАЙНЕРА XAML)
        // Викликає основний конструктор, передаючи null для _navigationService.
        public LoginViewModel() : this(null)
        {
        }

        // ✅ 2. ОСНОВНИЙ КОНСТРУКТОР (ДЛЯ DI-КОНТЕЙНЕРА)
        public LoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        // **********************************************
        // КОМАНДИ
        // **********************************************

        [RelayCommand]
        private void Login(object parameter)
        {
            string password = "";

            if (parameter is PasswordBox pb)
            {
                password = pb.Password;
            }

            if (Email == "test@game.com" && password == "123")
            {
                MessageBox.Show("Успішний вхід!", "Успіх");

                // ✅ ДОДАНО: ПЕРЕВІРКА НА NULL ПЕРЕД ВИКЛИКОМ НАВІГАЦІЇ
                if (_navigationService != null)
                {
                    _navigationService.NavigateTo<MainPageViewModel>();
                }
                else
                {
                    // Це означає, що програма була запущена неправильно або ViewModel створено вручну.
                    MessageBox.Show("Критична помилка: Сервіс навігації не ініціалізовано.", "Помилка DI");
                }
            }
            else
            {
                MessageBox.Show("Невірний email або пароль.", "Помилка");
            }
        }

        [RelayCommand]
        private void GoToRegister()
        {
            // ✅ ДОДАНО: ПЕРЕВІРКА НА NULL ПЕРЕД ВИКЛИКОМ НАВІГАЦІЇ
            if (_navigationService != null)
            {
                _navigationService.NavigateTo<RegisterViewModel>();
            }
        }
    }
}