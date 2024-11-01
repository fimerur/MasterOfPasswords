using MasterOfPasswords.Encryption;
using MasterOfPasswords.PasswordSetter;

namespace MasterOfPasswords.Storage
{
    public interface IPasswordStore
    {
        Task AddPasswordToFileAsync(PasswordEntry entry);
        Task<PasswordEntry> FindPasswordByLoginAsync(string login);
        Task UpdatePasswordInFileAsync(string login, string newPassword, IEncryptor encryptor);
    }
}