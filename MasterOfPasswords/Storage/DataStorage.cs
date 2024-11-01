using MasterOfPasswords.Encryption;
using MasterOfPasswords.PasswordSetter;
using Newtonsoft.Json;

namespace MasterOfPasswords.Storage;

public class DataStorage : IDataStorage
{
    private const string FilePath = "/Users/Fimeru/Desktop/PM storage/passwords.json";

    public async Task SaveAsync(List<PasswordEntry> passwordList)
    {
        var json = JsonConvert.SerializeObject(passwordList, Formatting.Indented);
        await File.WriteAllTextAsync(FilePath, json);
    }

    public async Task<List<PasswordEntry>> LoadAsync()
    {
        if (!File.Exists(FilePath))
            return new List<PasswordEntry>();

        var json = await File.ReadAllTextAsync(FilePath);
        return JsonConvert.DeserializeObject<List<PasswordEntry>>(json) ?? new List<PasswordEntry>();
    }

    public async Task<PasswordEntry?> FindPasswordInFileAsync(string login)
    {
        var passwordList = await LoadAsync();
        return passwordList.FirstOrDefault(p => p.Login == login);
    }

    public async Task UpdatePasswordInFileAsync(string login, string newPassword, IEncryptor encryptor)
    {
        var passwordList = await LoadAsync();
        var entry = passwordList.FirstOrDefault(p => p.Login == login);

        if (entry != null)
        {
            entry.SetEncryptedPassword(newPassword, encryptor);
            await SaveAsync(passwordList);
        }
    }
}