using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Services;
using Presentation.Models;
using GameOverDose.BLL.Interfaces;
using System.Windows;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Presentation.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IUserGameService _userGameService; // Нова залежність

        [ObservableProperty]
        private User currentUser = new User { Username = "Завантаження...", Email = "" };

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string totalPlaytime = "0 годин";

        [ObservableProperty]
        private int gamesCount = 0;

        // Колекція ігор для відображення
        public ObservableCollection<GameModel> UserGames { get; } = new ObservableCollection<GameModel>();

        public ProfileViewModel() : this(null, null, null, null) { }

        // ОНОВЛЕНИЙ КОНСТРУКТОР: Додаємо IUserGameService
        public ProfileViewModel(
            INavigationService navigationService,
            IUserService userService,
            IAuthService authService,
            IUserGameService userGameService)
        {
            _navigationService = navigationService;
            _userService = userService;
            _authService = authService;
            _userGameService = userGameService;

            if (_userService != null && _authService != null && _userGameService != null)
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

                    // Оновлення загального часу та кількості ігор
                    GamesCount = user.UserGames?.Count ?? 0;
                    TotalPlaytime = $"{user.UserGames?.Sum(ug => ug.Hours) ?? 0} годин";

                    // ----------------------------------------------------
                    // ЛОГІКА ЗАВАНТАЖЕННЯ ІГОР З БЕКЕНДУ
                    // ----------------------------------------------------

                    var userGamesFromService = await _userGameService.GetUserGamesAsync(user.Nickname);

                    UserGames.Clear();

                    if (userGamesFromService != null)
                    {
                        foreach (var ug in userGamesFromService.Where(ug => ug.Game != null))
                        {
                            // МАПІНГ DAL (UserGame, Game) -> Presentation (GameModel)
                            UserGames.Add(new GameModel
                            {
                                Id = ug.Game.Id,
                                Title = ug.Game.Name,
                                Name = ug.Game.Slug,
                                HoursPlayed = ug.Hours,
                                Status = ug.Status,

                                // Приклад: інші властивості, якщо вони є у вашій DAL-сутності Game
                                ImageSource = ug.Game.BackgroundImage ?? string.Empty, 
                                // Genre = ug.Game.Genre.Name ?? string.Empty,
                            });
                        }
                    }
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

        // Команда для запуску гри
        [RelayCommand]
        private void LaunchGame(GameModel game)
        {
            if (game != null)
            {
                MessageBox.Show($"Запуск гри: {game.Title}...", "Гра починається");
            }
        }


        // Існуючі команди
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