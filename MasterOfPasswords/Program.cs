using MasterOfPasswords.App;
using MasterOfPasswords.Commands;
using MasterOfPasswords.Storage;
using MasterOfPasswords.User;
using MasterOfPasswords.Postgres;
using Microsoft.EntityFrameworkCore;

namespace MasterOfPasswords;

class Program
{
    private const string MasterPassword = "7891";

    static async Task Main(string[] args)
    {
        var dbContext = new ApplicationDbContext();
            
        await dbContext.Database.MigrateAsync();
            
        IPasswordStore passwordStore = new PasswordStore(dbContext);
        IUser user = new User.User(MasterPassword);
            
        IConsoleManager consoleManager = new ConsoleManager();
        IPasswordManagerApp app = new PasswordManagerApp(user, passwordStore, consoleManager);
            
        if (await app.AuthenticateUserAsync())
        {
            await app.StartMenuAsync();
        }
    }
}