using MasterOfPasswords.App;

namespace MasterOfPasswords.Commands
{
    public class MenuHandler
    {
        private readonly IPasswordManagerApp _passwordManagerApp; // Используем интерфейс
        private readonly IConsoleManager _consoleManager; // Добавлен интерфейс для консоли

        public MenuHandler(IPasswordManagerApp passwordManagerApp, IConsoleManager consoleManager)
        {
            _passwordManagerApp = passwordManagerApp; // Заменено на интерфейс
            _consoleManager = consoleManager; // Передаем интерфейс
        }

        public async Task DisplayMenuAsync()
        {
            while (true)
            {
                _consoleManager.WriteInformation("\nВыберите действие: 1) Добавить пароль 2) Найти пароль 3) Изменить пароль 4) Выход");
                string option = _consoleManager.ReadInput();

                if (option == "1")
                {
                    await _passwordManagerApp.AddPasswordAsync();
                }
                else if (option == "2")
                {
                    await _passwordManagerApp.FindPasswordAsync();
                }
                else if (option == "3")
                {
                    await _passwordManagerApp.UpdatePasswordAsync();
                }
                else if (option == "4")
                {
                    break;
                }
                else
                {
                    _consoleManager.WriteError("Неверный формат ввода.");
                }
            }
            _consoleManager.WriteInformation("До свидания!");
        }
    }
}