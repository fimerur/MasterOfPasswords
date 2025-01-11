using System;
using Avalonia.Controls;
using MasterOfPasswords.UI.ViewModels;
using MasterOfPasswords.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MasterOfPasswords.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            // Используем контейнер зависимостей для получения MainViewModel
            var credentialsService = serviceProvider.GetRequiredService<ICredentialsService>();
            DataContext = new MainViewModel(credentialsService);
        }
    }
}