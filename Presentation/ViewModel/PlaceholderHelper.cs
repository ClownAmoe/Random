using System.Windows;
using System.Windows.Controls;

namespace Presentation.ViewModels // або Presentation.Helpers
{
    // Статичний клас для визначення приєднаної властивості Placeholder
    public static class PlaceholderHelper
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(PlaceholderHelper), new PropertyMetadata(null));

        public static string GetPlaceholder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderProperty);
        }

        public static void SetPlaceholder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderProperty, value);
        }
    }
}