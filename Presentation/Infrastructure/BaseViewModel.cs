// Presentation/Infrastructure/BaseViewModel.cs
#nullable enable
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Presentation.Infrastructure;

// Базовий клас, який реалізує інтерфейс INotifyPropertyChanged для реактивності UI
public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}