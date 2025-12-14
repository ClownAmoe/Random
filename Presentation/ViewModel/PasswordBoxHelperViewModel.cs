// Файл: ViewModels/PasswordBoxHelper.cs (виправлення CS0103, CS8604, CS8600)

using System.Windows;
using System.Windows.Controls;

namespace Presentation.ViewModels
{
    // Клас перейменовано на PasswordBoxHelper, як у коді
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
                typeof(string), typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
                typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false, OnAttachPropertyChanged));

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
                typeof(PasswordBoxHelper));

        public static void SetAttach(DependencyObject dp, bool value) => dp.SetValue(AttachProperty, value);
        public static bool GetAttach(DependencyObject dp) => (bool)dp.GetValue(AttachProperty);

        // CS8600 / CS8604: Приведення (string)dp.GetValue може бути null. 
        // Припускаючи, що GetPassword повертає string.Empty, якщо не встановлено.
        public static string GetPassword(DependencyObject dp) => (string)dp.GetValue(PasswordProperty) ?? string.Empty;
        public static void SetPassword(DependencyObject dp, string value) => dp.SetValue(PasswordProperty, value);

        private static bool GetIsUpdating(DependencyObject dp) => (bool)dp.GetValue(IsUpdatingProperty);
        private static void SetIsUpdating(DependencyObject dp, bool value) => dp.SetValue(IsUpdatingProperty, value);

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox? passwordBox = sender as PasswordBox; // Додано '?'

            // 1. Захист від NullReference
            if (passwordBox == null)
                return;

            // Відписуємося від події, щоб запобігти рекурсії
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!GetIsUpdating(passwordBox))
            {
                // 2. Встановлюємо нове значення, переконавшись, що воно не null
                string? newPassword = e.NewValue as string; // Додано '?'

                // Встановлюємо Password, тільки якщо значення відрізняється, щоб мінімізувати виклики
                if (passwordBox.Password != newPassword)
                {
                    // Якщо newPassword null, використовуємо string.Empty
                    passwordBox.Password = newPassword ?? string.Empty;
                }
            }

            // Підписуємося назад
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void OnAttachPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox? passwordBox = sender as PasswordBox; // Додано '?'

            if (passwordBox != null)
            {
                if ((bool)e.OldValue)
                {
                    passwordBox.PasswordChanged -= PasswordChanged;
                }

                if ((bool)e.NewValue)
                {
                    passwordBox.PasswordChanged += PasswordChanged;
                }
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            // ✅ ВИПРАВЛЕННЯ CS0103: 'dp' та 'value' не існували тут.
            // Ми використовуємо passwordBox та його Password.
            PasswordBox? passwordBox = sender as PasswordBox; // Додано '?'

            if (passwordBox == null) return;

            // ✅ ВИПРАВЛЕННЯ CS8604 та CS0103: Правильне використання passwordBox
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);

            // Видалено помилкові рядки:
            // PasswordBoxHelper.SetIsUpdating(dp!, value);
            // SetPassword(passwordBox, passwordBox.Password); // Дублювання
            // SetIsUpdating(passwordBox, false); // Дублювання
        }
    }
}