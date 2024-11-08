using MasterOfPasswords.Encryption;
using Microsoft.EntityFrameworkCore;
using MasterOfPasswords.PasswordSetter;
using MasterOfPasswords.Postgres;

namespace MasterOfPasswords.Storage
{
    public class PasswordStore : IPasswordStore
    {
        private readonly ApplicationDbContext _dbContext;

        public PasswordStore(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPasswordToFileAsync(PasswordEntry entry)
        {
            try
            {
                var dbCredential = entry.ToDbCredential();
                await _dbContext.Credentials.AddAsync(dbCredential);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Ошибка при сохранении изменений: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<PasswordEntry> FindPasswordByLoginAsync(string login)
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
}