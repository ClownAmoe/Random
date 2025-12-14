// Файл: ViewModels/ShellViewModel.cs (ФІНАЛЬНО ВИПРАВЛЕНО)

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Services;
using Presentation.ViewModels;

namespace Presentation.ViewModels
{
    // ShellViewModel керує головною рамкою та навігацією
    public partial class ShellViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        // Властивість, до якої буде прив'язаний ContentControl у MainWindow.xaml
        [ObservableProperty]
        private ObservableObject currentViewModel;

        // ✅ НОВА ВЛАСТИВІСТЬ: Керує видимістю навігаційного хедеру у MainWindow.xaml
        [ObservableProperty]
        private bool isHeaderVisible = false;

        public ShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            _navigationService.CurrentViewModelChanged += HandleNavigationChanged;

            // ⚠️ ВАЖЛИВО: Ініціюємо навігацію ПЕРЕД встановленням CurrentViewModel
            // Це гарантує, що CurrentViewModel не буде null і буде встановлено LoginViewModel
            _navigationService.NavigateTo<LoginViewModel>();

            CurrentViewModel = _navigationService.CurrentViewModel;

            // Встановлюємо початковий стан видимості (LoginView -> приховано)
            isHeaderVisible = CurrentViewModel is not LoginViewModel;
        }

        // ... (Команди залишаються без змін)

        [RelayCommand]
        private void GoBack()
        {
            _navigationService.GoBack();
        }

        [RelayCommand]
        private void GoHome()
        {
            _navigationService.NavigateTo<MainPageViewModel>();
        }

        [RelayCommand]
        private void GoToProfile()
        {
            _navigationService.NavigateTo<ProfileViewModel>();
        }

        // ... (Кінець команд)


        // ✅ ВИПРАВЛЕНО: Додана логіка для контролю видимості меню
        private void HandleNavigationChanged(ObservableObject viewModel)
        {
            CurrentViewModel = viewModel;

            // ЛОГІКА ВИДИМОСТІ:
            // Меню видиме, якщо поточна сторінка НЕ LoginView і НЕ RegisterView.
            // Тобто, якщо ви перейшли на MainPageViewModel, IsHeaderVisible стане true.
            IsHeaderVisible = viewModel is not LoginViewModel && viewModel is not RegisterViewModel;
        }
    }
}