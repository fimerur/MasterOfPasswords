using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MasterOfPasswords.UI.ViewModels;
using MasterOfPasswords.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MasterOfPasswords.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Получаем экземпляр ICredentialsService из контейнера зависимостей
            var credentialsService = App.Services.GetRequiredService<ICredentialsService>();

            // Передаем credentialsService в конструктор MainViewModel
            DataContext = new MainViewModel(credentialsService);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}