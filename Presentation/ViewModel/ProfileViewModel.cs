// Файл: ViewModels/ProfileViewModel.cs (ПОВНИЙ КОД)

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Services;
using Presentation.Models;
using System.Windows;

namespace Presentation.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        // Властивість, до якої прив'язується XAML (CurrentUser.Username, CurrentUser.Email)
        // В ідеалі, це повинно завантажуватися зі служби автентифікації
        [ObservableProperty]
        private User currentUser = new User { Username = "Іван_Геймер", Email = "ivan@gameoverdose.com" };

        [ObservableProperty]
        private bool isLoading = false;

        // **********************************************
        // КОНСТРУКТОРИ
        // **********************************************

        // ✅ 1. БЕЗПАРАМЕТРИЧНИЙ КОНСТРУКТОР (ДЛЯ ДИЗАЙНЕРА XAML)
        public ProfileViewModel() : this(null) { }

        // ✅ 2. ОСНОВНИЙ КОНСТРУКТОР (ДЛЯ DI-КОНТЕЙНЕРА)
        public ProfileViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        // **********************************************
        // КОМАНДИ
        // **********************************************

        [RelayCommand]
        private void Logout()
        {
            MessageBox.Show("Ви вийшли з профілю.", "Вихід");

            // ✅ Безпечна навігація: Повернення на сторінку логіну
            if (_navigationService != null)
            {
                _navigationService.NavigateTo<LoginViewModel>();
            }
        }

        // Тимчасова заглушка команди
        [RelayCommand]
        private void ChangePassword()
        {
            MessageBox.Show("Функція зміни пароля поки що недоступна.", "Налаштування");
        }

        // Тимчасова заглушка команди
        [RelayCommand]
        private void ManageData()
        {
            MessageBox.Show("Функція керування даними поки що недоступна.", "Налаштування");
        }
    }
}