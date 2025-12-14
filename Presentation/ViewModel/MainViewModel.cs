// Файл: ViewModels/MainViewModel.cs (ФІНАЛЬНЕ ВИПРАВЛЕННЯ)

using CommunityToolkit.Mvvm.ComponentModel;
using Presentation.Services;
using System;

namespace Presentation.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableObject currentViewModel;

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // ✅ ВИПРАВЛЕНО: Використовуємо лямбда-вираз (=>) для підписки.
            // Це усуває конфлікт імені методу.
            _navigationService.CurrentViewModelChanged += (viewModel) =>
            {
                CurrentViewModel = viewModel;
            };
        }

        // ❌ ВИДАЛИТИ: Старий ручний метод OnCurrentViewModelChanged
        /* private void OnCurrentViewModelChanged(ObservableObject viewModel)
        {
            CurrentViewModel = viewModel;
        }
        */
    }
}