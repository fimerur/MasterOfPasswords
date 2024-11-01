using MasterOfPasswords.Encryption;
using MasterOfPasswords.PasswordSetter;

namespace MasterOfPasswords.Storage
{
    public interface IDataStorage
    {
        Task SaveAsync(List<PasswordEntry> passwordList); // Асинхронное сохранение списка паролей
        Task<List<PasswordEntry>> LoadAsync(); // Асинхронная загрузка списка паролей
        Task<PasswordEntry?> FindPasswordInFileAsync(string login); // Асинхронный поиск пароля
        Task UpdatePasswordInFileAsync(string login, string newPassword, IEncryptor encryptor); // Асинхронное обновление пароля
    }
}