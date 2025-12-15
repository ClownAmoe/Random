// Presentation/ViewModels/RegisterViewModel.cs (ВИПРАВЛЕНО: підключення до БД)

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Services;
using GameOverDose.BLL.Interfaces;
using GameOverDose.DAL.Entities;
using System.Windows;
using System.Threading.Tasks;
using System;

namespace Presentation.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;

        // **********************************************
        // ВЛАСТИВОСТІ
        // **********************************************

        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        // **********************************************
        // КОНСТРУКТОРИ
        // **********************************************

        // ✅ Безпараметричний для дизайнера XAML
        public RegisterViewModel() : this(null, null) { }

        // ✅ Основний конструктор для DI
        public RegisterViewModel(INavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
        }

        // **********************************************
        // КОМАНДИ
        // **********************************************

        [RelayCommand]
        private async Task RegisterAsync()
        {
            ErrorMessage = string.Empty;

            // Валідація
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Введіть ім'я користувача";
                MessageBox.Show(ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Введіть email";
                MessageBox.Show(ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Введіть пароль";
                MessageBox.Show(ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Паролі не співпадають";
                MessageBox.Show(ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Password.Length < 3)
            {
                ErrorMessage = "Пароль має містити мінімум 3 символи";
                MessageBox.Show(ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsLoading = true;

            try
            {
                // Перевірка, чи користувач вже існує
                var existingUser = await _userService.GetUserByNicknameAsync(Username);
                if (existingUser != null)
                {
                    ErrorMessage = "Користувач з таким нікнеймом вже існує";
                    MessageBox.Show(ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Створення нового користувача
                var newUser = new User
                {
                    Nickname = Username,
                    Email = Email,
                    Password = Password, // ⚠️ В реальному застосунку хешуйте пароль!
                    Lvl = 1,
                    Avatar = "default_avatar.png"
                };

                await _userService.CreateUserAsync(newUser);

                MessageBox.Show($"Користувач {Username} успішно зареєстрований!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

                // Перехід на сторінку логіну
                if (_navigationService != null)
                {
                    _navigationService.NavigateTo<LoginViewModel>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Помилка реєстрації: {ex.Message}";
                MessageBox.Show(ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Registration error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void GoToLogin()
        {
            if (_navigationService != null)
            {
                _navigationService.NavigateTo<LoginViewModel>();
            }
        }
    }
}