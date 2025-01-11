using MasterOfPasswords.Domain;
using MasterOfPasswords.Models;
using ReactiveUI;
using System;
using System.Reactive;
using MasterOfPasswords.Domain.Interfaces;

namespace MasterOfPasswords.UI.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private readonly ICredentialsService _credentialsService;
        private string _login;
        private string _password;

        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public ReactiveCommand<Unit, Unit> AddPasswordCommand { get; }
        public ReactiveCommand<Unit, Unit> GetPasswordCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdatePasswordCommand { get; }

        public MainViewModel(ICredentialsService credentialsService)
        {
            _credentialsService = credentialsService;

            // Инициализация команд
            AddPasswordCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    var credentialDto = new CredentialDto(Login, Password); // Передаем данные в конструктор
                    await _credentialsService.AddPassword(credentialDto);
                }
                catch (Exception ex)
                {
                    // Обработать исключение, например, отобразить сообщение
                    Console.WriteLine($"Error adding password: {ex.Message}");
                }
            });

            GetPasswordCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    var credential = await _credentialsService.GetPassword(Login);
                    Password = credential.Password; // Устанавливаем полученный пароль
                }
                catch (Exception ex)
                {
                    // Обработать исключение, например, отобразить сообщение
                    Console.WriteLine($"Error getting password: {ex.Message}");
                }
            });

            UpdatePasswordCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    var updatedCredentialDto = new CredentialDto(Login, Password); // Передаем обновленные данные
                    await _credentialsService.UpdatePassword(updatedCredentialDto);
                }
                catch (Exception ex)
                {
                    // Обработать исключение, например, отобразить сообщение
                    Console.WriteLine($"Error updating password: {ex.Message}");
                }
            });
        }
    }
}
