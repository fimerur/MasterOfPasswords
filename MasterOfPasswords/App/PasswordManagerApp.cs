using MasterOfPasswords.Commands;
using MasterOfPasswords.Encryption;
using MasterOfPasswords.PasswordSetter;
using MasterOfPasswords.Storage;
using MasterOfPasswords.User;

namespace MasterOfPasswords.App;

public class PasswordManagerApp : IPasswordManagerApp
{
    private readonly IUser _user;
    private readonly IPasswordStore _passwordStore;
    private readonly IConsoleManager _consoleManager; // Используем интерфейс
    private readonly MenuHandler _menuHandler;
    private readonly Encryptor _encryptor;

    private const int MaxAttempts = 5;
    
    public PasswordManagerApp(IUser user, IPasswordStore passwordStore, IConsoleManager consoleManager)
    {
        _user = user;
        _passwordStore = passwordStore;
        _consoleManager = consoleManager; // Передаем интерфейс
        _menuHandler = new MenuHandler(this, consoleManager);
        _encryptor = new Encryptor();
    }

    public async Task<bool> AuthenticateUserAsync()
    {
        int attempts = 0;

        while (attempts < MaxAttempts)
        {
            _consoleManager.WriteInformation("Введите мастер-пароль: ");
            string masterPasswordInput = _consoleManager.ReadInput();

            if (_user.Authenticate(masterPasswordInput))
            {
                _consoleManager.WriteInformation("Аутентификация успешна.");
                return true;
            }

            attempts++;
            _consoleManager.WriteError($"Неверный мастер-пароль. Осталось попыток: {MaxAttempts - attempts}");
        }

        _consoleManager.WriteError("Программа завершена из-за превышения количества попыток.");
        return false;
    }

    public async Task StartMenuAsync()
    {
        await _menuHandler.DisplayMenuAsync();
    }

    public async Task AddPasswordAsync()
    {
        try
        {
            _consoleManager.WriteInformation("Введите логин: ");
            string login = _consoleManager.ReadInput();

            if (await _passwordStore.FindPasswordByLoginAsync(login) != null)
            {
                _consoleManager.WriteWarning("Этот логин уже существует.");
                return;
            }

            _consoleManager.WriteInformation("Введите пароль: ");
            string password = _consoleManager.ReadInput();

            var entry = new PasswordEntry(login, password, _encryptor);
            await _passwordStore.AddPasswordToFileAsync(entry);
            _consoleManager.WriteInformation("Пароль успешно добавлен.");
        }
        catch (Exception ex)
        {
            _consoleManager.WriteError($"Произошла ошибка при добавлении пароля: {ex.Message}");
        }
    }

    public async Task FindPasswordAsync()
    {
        try
        {
            _consoleManager.WriteInformation("Введите логин для поиска пароля: ");
            string login = _consoleManager.ReadInput();
            var entry = await _passwordStore.FindPasswordByLoginAsync(login);

            if (entry != null)
            {
                string decryptedPassword = entry.GetDecryptedPassword(_encryptor);
                _consoleManager.WriteInformation($"Пароль для {login}: {decryptedPassword}");
            }
            else
            {
                _consoleManager.WriteInformation("Логин не найден.");
            }
        }
        catch (Exception ex)
        {
            _consoleManager.WriteError($"Произошла ошибка при поиске пароля: {ex.Message}");
        }
    }

    public async Task UpdatePasswordAsync()
    {
        try
        {
            _consoleManager.WriteInformation("Введите логин для изменения соответствующего ему пароля: ");
            string login = _consoleManager.ReadInput();
            
            var entry = await _passwordStore.FindPasswordByLoginAsync(login);
            if (entry == null)
            {
                _consoleManager.WriteInformation("Логин не найден.");
                return;
            }

            _consoleManager.WriteInformation("Введите новый пароль: ");
            string newPassword = _consoleManager.ReadInput();

            await _passwordStore.UpdatePasswordInFileAsync(login, newPassword, _encryptor);
            _consoleManager.WriteInformation("Пароль успешно обновлен.");
        }
        catch (Exception ex)
        {
            _consoleManager.WriteError($"Произошла ошибка при обновлении пароля: {ex.Message}");
        }
    }
}
