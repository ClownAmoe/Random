using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Presentation.ViewModels;

namespace Presentation.Views
{
    public partial class GameDetailsView : UserControl
    {
        public GameDetailsView()
        {
            InitializeComponent();
        }

        private void OpenTrailerButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is GameDetailsViewModel viewModel &&
                !string.IsNullOrEmpty(viewModel.CurrentGame?.TrailerUrl))
            {
                try
                {
                    var trailerUrl = viewModel.CurrentGame.TrailerUrl;

                    // Конвертуємо embed URL в звичайний YouTube URL
                    var watchUrl = trailerUrl.Replace("/embed/", "/watch?v=");

                    // Відкриваємо в браузері за замовчуванням
                    var psi = new ProcessStartInfo
                    {
                        FileName = watchUrl,
                        UseShellExecute = true
                    };
                    Process.Start(psi);

                    Debug.WriteLine($"✅ Відкрито трейлер: {watchUrl}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Не вдалося відкрити трейлер: {ex.Message}",
                        "Помилка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    Debug.WriteLine($"❌ Помилка відкриття трейлера: {ex}");
                }
            }
            else
            {
                MessageBox.Show(
                    "Трейлер для цієї гри недоступний",
                    "Інформація",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}