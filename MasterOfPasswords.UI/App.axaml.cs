using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MasterOfPasswords.Domain.Interfaces;
using MasterOfPasswords.Domain;
using MasterOfPasswords.Postgres;
using MasterOfPasswords.UI.Views;
using MasterOfPasswords.Encryption;
using MasterOfPasswords.UI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MasterOfPasswords.UI
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var serviceCollection = new ServiceCollection();
                
                serviceCollection.AddSingleton<IEncryptor, Encryptor>();
                serviceCollection.AddSingleton<ICredentialsService, CredentialsService>();
                
                serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=Credentials"));
                
                Services = serviceCollection.BuildServiceProvider();
                
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel(Services.GetRequiredService<ICredentialsService>())
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}