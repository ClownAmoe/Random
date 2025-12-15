using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Services;
using Presentation.Models;
using GameOverDose.BLL.Interfaces;
using System.Windows;
using System.Threading.Tasks;
using System;

namespace Presentation.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private User currentUser = new User { Username = "Завантаження...", Email = "" };

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string totalPlaytime = "0 годин";

        [ObservableProperty]
        private int gamesCount = 0;

        public ProfileViewModel() : this(null, null, null) { }

        public ProfileViewModel(INavigationService navigationService, IUserService userService, IAuthService authService)
        {
            _navigationService = navigationService;
            _userService = userService;
            _authService = authService;

            if (_userService != null && _authService != null)
            {
                LoadUserDataAsync();
            }
        }

        private async Task LoadUserDataAsync()
        {
            if (!_authService.IsAuthenticated || !_authService.CurrentUserId.HasValue)
            {
                MessageBox.Show("Користувач не авторизований", "Помилка");
                _navigationService?.NavigateTo<LoginViewModel>();
                return;
            }

            IsLoading = true;

            try
            {
                var user = await _userService.GetUserWithGamesAsync(_authService.CurrentUserId.Value);

                if (user != null)
                {
                    CurrentUser = new User
                    {
                        Username = user.Nickname,
                        Email = user.Email
                    };

                    GamesCount = user.UserGames?.Count ?? 0;

                    int totalHours = 0;
                    if (user.UserGames != null)
                    {
                        foreach (var ug in user.UserGames)
                        {
                            totalHours += ug.Hours;
                        }
                    }

                    TotalPlaytime = $"{totalHours} годин";
                }
                else
                {
                    MessageBox.Show("Не вдалося завантажити дані профілю", "Помилка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження профілю: {ex.Message}", "Помилка");
                System.Diagnostics.Debug.WriteLine($"Profile load error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void Logout()
        {
            _authService?.Logout();
            MessageBox.Show("Ви вийшли з профілю.", "Вихід");

            if (_navigationService != null)
            {
                _navigationService.NavigateTo<LoginViewModel>();
            }
        }

        [RelayCommand]
        private void ChangePassword()
        {
            MessageBox.Show("Функція зміни пароля поки що недоступна.", "Налаштування");
        }

        [RelayCommand]
        private void ManageData()
        {
            MessageBox.Show("Функція керування даними поки що недоступна.", "Налаштування");
        }
    }
}