using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Models;
using Presentation.Services;
using GameOverDose.BLL.Interfaces;
using GameOverDose.DAL.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public partial class GameDetailsViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IGameService _gameService;
        private readonly ICommentService _commentService;
        private readonly IAuthService _authService;
        private readonly IUserGameService _userGameService;

        private int _currentGameId;

        [ObservableProperty]
        private GameModel currentGame = new GameModel();

        [ObservableProperty]
        private ObservableCollection<GameComment> comments = new();

        [ObservableProperty]
        private string newCommentText = string.Empty;

        [ObservableProperty]
        private int newCommentRating = 5;

        [ObservableProperty]
        private bool isTracking = false;

        [ObservableProperty]
        private ObservableCollection<RatingStar> ratingStars = new();

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private bool hasTrailer = false;

        public GameDetailsViewModel() : this(null, null, null, null, null)
        {
            CurrentGame = new GameModel
            {
                Name = "Cyberpunk 2077",
                Description = "Велика рольова гра у футуристичному Night City.",
                Price = 59.99m,
                TrailerUrl = "https://www.youtube.com/embed/8X2kIfS6fb8",
                ImageSource = "https://media.rawg.io/media/games/26d/26d4437715bee60138dab4a7c8c59c92.jpg"
            };
            HasTrailer = !string.IsNullOrEmpty(CurrentGame.TrailerUrl);
            InitializeRatingStars();
        }

        public GameDetailsViewModel(
            INavigationService navigationService,
            IGameService gameService,
            ICommentService commentService,
            IAuthService authService,
            IUserGameService userGameService)
        {
            _navigationService = navigationService;
            _gameService = gameService;
            _commentService = commentService;
            _authService = authService;
            _userGameService = userGameService;
            InitializeRatingStars();
        }

        public async Task LoadGameAsync(int gameId)
        {
            _currentGameId = gameId;
            IsLoading = true;

            try
            {
                var dalGame = await _gameService.GetGameByIdAsync(gameId);

                if (dalGame != null)
                {
                    CurrentGame = new GameModel
                    {
                        Id = dalGame.Id,
                        Title = dalGame.Name,
                        Name = dalGame.Name,
                        Description = dalGame.Description,
                        Price = dalGame.Price ?? 0m,
                        ImageSource = dalGame.BackgroundImage ?? string.Empty,
                        TrailerUrl = dalGame.TrailerUrl ?? string.Empty,
                        Developers = ExtractDevelopers(dalGame),
                        ReleaseDate = dalGame.Release ?? DateTime.MinValue
                    };

                    HasTrailer = !string.IsNullOrEmpty(CurrentGame.TrailerUrl);

                    System.Diagnostics.Debug.WriteLine($"✅ Game loaded: {CurrentGame.Name}");
                    System.Diagnostics.Debug.WriteLine($"✅ Trailer URL: {CurrentGame.TrailerUrl}");
                    System.Diagnostics.Debug.WriteLine($"✅ HasTrailer: {HasTrailer}");

                    if (_authService?.IsAuthenticated == true && _authService.CurrentUserId.HasValue)
                    {
                        await LoadTrackingStatusAsync();
                    }
                }
                else
                {
                    _currentGameId = 0;
                    MessageBox.Show("Запитувана гра не знайдена в базі даних.", "Помилка завантаження",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                await LoadCommentsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження гри: {ex.Message}", "Помилка");
                System.Diagnostics.Debug.WriteLine($"Game load error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadTrackingStatusAsync()
        {
            if (_currentGameId == 0 || !_authService.CurrentUserId.HasValue) return;

            try
            {
                IsTracking = await _userGameService.GetTrackingStatusAsync(
                    _authService.CurrentUserId.Value,
                    _currentGameId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Tracking status load error: {ex}");
            }
        }

        private List<string> ExtractDevelopers(GameOverDose.DAL.Entities.Game game)
        {
            if (string.IsNullOrEmpty(game.Platforms))
                return new List<string> { "Розробник невідомий" };

            return game.Platforms.Split(',')
                .Select(p => p.Trim())
                .Take(3)
                .ToList();
        }

        private async Task LoadCommentsAsync()
        {
            if (_currentGameId == 0) return;

            try
            {
                var dalComments = await _commentService.GetCommentsByGameAsync(_currentGameId);

                Comments.Clear();
                foreach (var c in dalComments)
                {
                    Comments.Add(new GameComment
                    {
                        Author = c.User?.Nickname ?? "Анонім",
                        Text = c.Text ?? "Без тексту",
                        Date = c.CreatedAt,
                        Rating = c.Rating ?? 0
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Comments load error: {ex}");
            }
        }

        private void InitializeRatingStars()
        {
            RatingStars.Clear();
            for (int i = 1; i <= 10; i++)
            {
                RatingStars.Add(new RatingStar
                {
                    Value = i,
                    Color = i <= NewCommentRating ? "#FFC830" : "#444444"
                });
            }
        }

        [RelayCommand]
        private void Follow()
        {
            MessageBox.Show($"Ви стежите за грою: {CurrentGame.Name}!", "Сповіщення");
        }

        [RelayCommand]
        private async Task ToggleTrackingAsync()
        {
            if (!_authService?.IsAuthenticated == true || !_authService.CurrentUserId.HasValue)
            {
                MessageBox.Show("Для відстеження гри потрібно увійти в систему", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentGameId == 0) return;

            try
            {
                IsTracking = !IsTracking;

                await _userGameService.UpdateTrackingStatusAsync(
                    _authService.CurrentUserId.Value,
                    _currentGameId,
                    IsTracking);

                MessageBox.Show(
                    IsTracking ? "Відстеження розпочато!" : "Відстеження зупинено!",
                    "Tracking"
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка оновлення статусу: {ex.Message}", "Помилка");
                System.Diagnostics.Debug.WriteLine($"Tracking toggle error: {ex}");
            }
        }

        [RelayCommand]
        private void SetRating(int rating)
        {
            NewCommentRating = rating;
            for (int i = 0; i < RatingStars.Count; i++)
            {
                RatingStars[i].Color = (i + 1) <= rating ? "#FFC830" : "#444444";
            }
        }

        [RelayCommand]
        private async Task PostCommentAsync()
        {
            if (!_authService?.IsAuthenticated == true || !_authService.CurrentUserId.HasValue)
            {
                MessageBox.Show("Для додавання коментаря потрібно увійти в систему", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentGameId == 0)
            {
                MessageBox.Show("Неможливо додати коментар: не визначено ID гри", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(NewCommentText))
            {
                MessageBox.Show("Введіть текст коментаря", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsLoading = true;

            try
            {
                var newComment = new Comment
                {
                    UserId = _authService.CurrentUserId.Value,
                    GameId = _currentGameId,
                    Text = NewCommentText,
                    Rating = NewCommentRating,
                    CreatedAt = DateTime.Now
                };

                await _commentService.CreateCommentAsync(newComment);
                await LoadCommentsAsync();

                NewCommentText = string.Empty;
                NewCommentRating = 5;
                InitializeRatingStars();

                MessageBox.Show("Коментар додано!", "Успіх");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка додавання коментаря: {ex.Message}", "Помилка");
                System.Diagnostics.Debug.WriteLine($"Comment post error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    public class GameComment
    {
        public string Author { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int Rating { get; set; }
    }

    public partial class RatingStar : ObservableObject
    {
        [ObservableProperty]
        private int value;

        [ObservableProperty]
        private string color = "#444444";
    }
}