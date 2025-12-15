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
        private readonly IAuthService _authService; // ✅ ДОДАНО

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public LoginViewModel() : this(null, null, null) { }

        // ✅ ОНОВЛЕНО: Додано IAuthService
        public LoginViewModel(INavigationService navigationService, IUserService userService, IAuthService authService)
        {
            _navigationService = navigationService;
            _userService = userService;
            _authService = authService;
        }

        [RelayCommand]
        private async Task LoginAsync(object parameter)
        {
            ErrorMessage = string.Empty;

            string password = "";
            if (parameter is PasswordBox pb)
            {
                password = pb.Password;
            }

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
                var user = await _userService.GetUserByNicknameAsync(Email);

                if (user == null)
                {
                    var allUsers = await _userService.GetAllUsersAsync();
                    user = allUsers.FirstOrDefault(u => u.Email.Equals(Email, StringComparison.OrdinalIgnoreCase));
                }

                if (user != null && user.Password == password)
                {
                    // ✅ ЗБЕРІГАЄМО ПОТОЧНОГО КОРИСТУВАЧА
                    _authService?.SetCurrentUser(user.Id, user.Nickname);

                    MessageBox.Show($"Ласкаво просимо, {user.Nickname}!", "Успішний вхід", MessageBoxButton.OK, MessageBoxImage.Information);

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
