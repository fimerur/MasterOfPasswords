using MasterOfPasswords.Encryption;
using MasterOfPasswords.PasswordSetter;

namespace MasterOfPasswords.Storage
{
    public interface IDataStorage
    {
        Task SaveAsync(List<PasswordEntry> passwordList);
        Task<List<PasswordEntry>> LoadAsync();
        Task<PasswordEntry?> FindPasswordInFileAsync(string login);
        Task UpdatePasswordInFileAsync(string login, string newPassword, IEncryptor encryptor);
    }
}