using System.Threading;
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
            
            var credentialsService = App.Services.GetRequiredService<ICredentialsService>();
            
            DataContext = new MainViewModel(credentialsService);


            var currentThread = Thread.CurrentThread.ManagedThreadId;


            var t = new Thread(x =>
            {
                var thread = Thread.CurrentThread.ManagedThreadId;
                var tb = this.FindControl<TextBlock>("Login");
                tb.Text = "My Silli Control";
            });
            
            t.Start();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}