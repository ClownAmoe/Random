// Файл: MainWindow.xaml.cs (ОЧИЩЕНО)
using System.Windows;
using Presentation.ViewModels;

namespace Presentation
{
    // MainWindow тепер просто контейнер, логіка навігації перенесена в MainViewModel/NavigationService
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // DataContext встановлюється в App.xaml.cs
        }
    }
}