// Файл: ViewModels/LoginViewModel.cs (ВИПРАВЛЕНО: підключення до БД)

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Services;
using System.Windows.Controls;
using System.Windows;
using GameOverDose.BLL.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Presentation.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        // **********************************************
        // КОНСТРУКТОРИ
        // **********************************************

        // ✅ 1. БЕЗПАРАМЕТРИЧНИЙ КОНСТРУКТОР (ДЛЯ ДИЗАЙНЕРА XAML)
        public LoginViewModel() : this(null, null)
        {
        }

        // ✅ 2. ОСНОВНИЙ КОНСТРУКТОР (ДЛЯ DI-КОНТЕЙНЕРА)
        public LoginViewModel(INavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
        }

        // **********************************************
        // КОМАНДИ
        // **********************************************

        [RelayCommand]
        private async Task LoginAsync(object parameter)
        {
            ErrorMessage = string.Empty;

            string password = "";
            if (parameter is PasswordBox pb)
            {
                password = pb.Password;
            }

            // Валідація
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Введіть email або нікнейм";
                MessageBox.Show(ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Введіть пароль";
                MessageBox.Show(ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsLoading = true;

            try
            {
                // ✅ ПІДКЛЮЧЕННЯ ДО БД: Перевірка користувача
                var user = await _userService.GetUserByNicknameAsync(Email);

                // Якщо не знайдено за нікнеймом, спробуємо за email
                if (user == null)
                {
                    var allUsers = await _userService.GetAllUsersAsync();
                    user = allUsers.FirstOrDefault(u => u.Email.Equals(Email, StringComparison.OrdinalIgnoreCase));
                }

                // Перевірка користувача та пароля
                if (user != null && user.Password == password)
                {
                    MessageBox.Show($"Ласкаво просимо, {user.Nickname}!", "Успішний вхід", MessageBoxButton.OK, MessageBoxImage.Information);

                    // ✅ НАВІГАЦІЯ: Перехід на головну сторінку
                    if (_navigationService != null)
                    {
                        _navigationService.NavigateTo<MainPageViewModel>();
                    }
                }
                else
                {
                    ErrorMessage = "Невірний email/нікнейм або пароль";
                    MessageBox.Show(ErrorMessage, "Помилка входу", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Помилка підключення: {ex.Message}";
                MessageBox.Show(ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Login error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void GoToRegister()
        {
            if (_navigationService != null)
            {
                _navigationService.NavigateTo<RegisterViewModel>();
            }
        }
    }
}