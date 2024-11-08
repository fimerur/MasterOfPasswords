using MasterOfPasswords.Encryption;
using MasterOfPasswords.PasswordSetter;
using MasterOfPasswords.Postgres;
using Microsoft.EntityFrameworkCore;

namespace MasterOfPasswords.Storage;

public class DataStorage : IDataStorage
{
    private readonly ApplicationDbContext _dbContext;

    public DataStorage(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(List<PasswordEntry> passwordList)
    {
        _dbContext.Credentials.RemoveRange(_dbContext.Credentials);
        await _dbContext.SaveChangesAsync();
        
        var dbCredentials = passwordList.Select(entry => entry.ToDbCredential()).ToList();
        await _dbContext.Credentials.AddRangeAsync(dbCredentials);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<PasswordEntry>> LoadAsync()
    {
        var dbCredentials = await _dbContext.Credentials.ToListAsync();
        return dbCredentials.Select(PasswordEntry.FromDbCredential).ToList();
    }

    public async Task<PasswordEntry?> FindPasswordInFileAsync(string login)
    {
        var dbCredential = await _dbContext.Credentials
            .Where(c => c.Login == login)
            .FirstOrDefaultAsync();

        return dbCredential == null ? null : PasswordEntry.FromDbCredential(dbCredential);
    }

    public async Task UpdatePasswordInFileAsync(string login, string newPassword, IEncryptor encryptor)
    {
        var dbCredential = await _dbContext.Credentials
            .Where(c => c.Login == login)
            .FirstOrDefaultAsync();

        if (dbCredential != null)
        {
            var entry = PasswordEntry.FromDbCredential(dbCredential);
            entry.SetEncryptedPassword(newPassword, encryptor);

            dbCredential.Password = entry.EncryptedPassword;
            await _dbContext.SaveChangesAsync();
        }
    }
}