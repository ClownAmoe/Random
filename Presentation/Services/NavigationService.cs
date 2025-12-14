// Файл: Services/NavigationService.cs (ФІНАЛЬНО ВИПРАВЛЕНО)

using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Presentation.ViewModels;

namespace Presentation.Services
{
    public class NavigationService : ObservableObject, INavigationService
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

            // ✅ ВИПРАВЛЕНО: Прибираємо некоректний коментар.
            // Початкова навігація ініціюється з ShellViewModel.
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