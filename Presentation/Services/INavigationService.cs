// Файл: Services/INavigationService.cs

using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Presentation.Services
{
    public interface INavigationService
    {
        // Подія, яка спрацьовує при зміні поточного ViewModel
        event Action<ObservableObject> CurrentViewModelChanged;

        // Властивість для отримання поточного ViewModel
        ObservableObject CurrentViewModel { get; }

        // Метод для навігації до нового ViewModel
        void NavigateTo<TViewModel>() where TViewModel : ObservableObject;

        // Метод для повернення назад у стеку
        bool GoBack();
    }
}