// Presentation/Services/NavigationService.cs (ОНОВЛЕНО)

using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Presentation.ViewModels;
using System.Threading.Tasks;

namespace Presentation.Services
{
    public class NavigationService : ObservableObject, INavigationService, IGameNavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Stack<Type> _history = new Stack<Type>();
        private ObservableObject _currentViewModel;

        public event Action<ObservableObject> CurrentViewModelChanged;

        public ObservableObject CurrentViewModel
        {
            get => _currentViewModel;
            private set => SetProperty(ref _currentViewModel, value);
        }

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ObservableObject
        {
            Type newViewModelType = typeof(TViewModel);

            if (_currentViewModel != null && _currentViewModel.GetType() != newViewModelType)
            {
                _history.Push(_currentViewModel.GetType());
            }

            CurrentViewModel = (ObservableObject)_serviceProvider.GetRequiredService(newViewModelType);
            CurrentViewModelChanged?.Invoke(CurrentViewModel);
        }

        // ✅ НОВИЙ МЕТОД: Навігація до GameDetails з передачею ID
        public async void NavigateToGameDetails(int gameId)
        {
            if (_currentViewModel != null)
            {
                _history.Push(_currentViewModel.GetType());
            }

            var viewModel = _serviceProvider.GetRequiredService<GameDetailsViewModel>();
            await viewModel.LoadGameAsync(gameId);

            CurrentViewModel = viewModel;
            CurrentViewModelChanged?.Invoke(CurrentViewModel);
        }

        public bool GoBack()
        {
            if (_history.Count > 0)
            {
                Type previousViewModelType = _history.Pop();
                CurrentViewModel = (ObservableObject)_serviceProvider.GetRequiredService(previousViewModelType);
                CurrentViewModelChanged?.Invoke(CurrentViewModel);
                return true;
            }
            return false;
        }
    }
}