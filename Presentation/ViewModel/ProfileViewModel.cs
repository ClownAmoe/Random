// Presentation/ViewModels/ProfileViewModel.cs (ВИПРАВЛЕНО: завантаження з БД)

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

        // ID поточного користувача (в реальному застосунку має братись з AuthService)
        private int _currentUserId = 2; // Тестовий користувач test@game.com

        [ObservableProperty]
        private User currentUser = new User { Username = "Завантаження...", Email = "" };

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string totalPlaytime = "0 годин";

        [ObservableProperty]
        private int gamesCount = 0;

        // **********************************************
        // КОНСТРУКТОРИ
        // **********************************************

        // ✅ Безпараметричний для дизайнера
        public ProfileViewModel() : this(null, null) { }

        // ✅ Основний конструктор для DI
        public ProfileViewModel(INavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;

            if (_userService != null)
            {
                LoadUserDataAsync();
            }
        }

        // **********************************************
        // ЗАВАНТАЖЕННЯ ДАНИХ
        // **********************************************

        private async Task LoadUserDataAsync()
        {
            IsLoading = true;

            try
            {
                // Завантаження користувача з іграми
                var user = await _userService.GetUserWithGamesAsync(_currentUserId);

                if (user != null)
                {
                    CurrentUser = new User
                    {
                        Username = user.Nickname,
                        Email = user.Email
                    };

                    // Підрахунок статистики
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

        // **********************************************
        // КОМАНДИ
        // **********************************************

        [RelayCommand]
        private void Logout()
        {
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