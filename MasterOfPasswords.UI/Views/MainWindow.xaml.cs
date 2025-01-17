using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MasterOfPasswords.UI.ViewModels;
using MasterOfPasswords.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MasterOfPasswords.UI.Views;

public partial class MainWindow : Window
{

    private TextBlock tb;
    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();
            
        var credentialsService = serviceProvider.GetRequiredService<ICredentialsService>();
        DataContext = new MainViewModel(credentialsService);
        
         tb  = this.FindControl<TextBlock>("Login");

        
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
       tb.Text = "TextTTTT";
    }
}