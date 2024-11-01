using System.Text.Json;
using MasterOfPasswords.Encryption;
using MasterOfPasswords.PasswordSetter;

namespace MasterOfPasswords.Storage
{
    public class PasswordStore : IPasswordStore
    {
        private readonly IDataStorage _storage;
        private const string FilePath = "passwords.json";

        public PasswordStore(IDataStorage storage)
        {
            _storage = storage;
        }

        public async Task AddPasswordToFileAsync(PasswordEntry entry)
        {
            var existingEntries = await LoadPasswordsFromFileAsync();
            existingEntries.Add(entry);
            await SavePasswordsToFileAsync(existingEntries);
        }

        public async Task<PasswordEntry> FindPasswordByLoginAsync(string login)
        {
            var entries = await LoadPasswordsFromFileAsync();
            return entries.FirstOrDefault(e => e.Login == login);
        }

        public async Task UpdatePasswordInFileAsync(string login, string newPassword, IEncryptor encryptor)
        {
            var entries = await LoadPasswordsFromFileAsync();
            var entry = entries.FirstOrDefault(e => e.Login == login);

            if (entry != null)
            {
                entry.SetEncryptedPassword(newPassword, encryptor);
                await SavePasswordsToFileAsync(entries);
            }
        }

        private async Task<List<PasswordEntry>> LoadPasswordsFromFileAsync()
        {
            if (!File.Exists(FilePath))
                return new List<PasswordEntry>();

            var json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<PasswordEntry>>(json) ?? new List<PasswordEntry>();
        }

        private async Task SavePasswordsToFileAsync(List<PasswordEntry> entries)
        {
            var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FilePath, json);
        }
    }
}
