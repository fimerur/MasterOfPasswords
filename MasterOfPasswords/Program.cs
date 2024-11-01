using MasterOfPasswords.App;
using MasterOfPasswords.Commands;
using MasterOfPasswords.Storage;
using MasterOfPasswords.User;

namespace MasterOfPasswords;

class Program
{
    private const string MasterPassword = "7891";

    static async Task Main(string[] args)
    {
        IDataStorage storage = new DataStorage();
        IPasswordStore passwordStore = new PasswordStore(storage);
        IUser user = new User.User(MasterPassword);
        
        IConsoleManager consoleManager = new ConsoleManager();
        IPasswordManagerApp app = new PasswordManagerApp(user, passwordStore, consoleManager);

        if (await app.AuthenticateUserAsync())
        {
            await app.StartMenuAsync();
        }
    }
}