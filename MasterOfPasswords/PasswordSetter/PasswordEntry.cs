using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using MasterOfPasswords.Encryption;

namespace MasterOfPasswords.PasswordSetter
{
    public class PasswordEntry
    {
        [JsonInclude]
        public string Login { get; private set; }

        [JsonInclude]
        public string? EncryptedPassword { get; private set; }

        [JsonInclude]
        public string Salt { get; private set; }

        [JsonConstructor]
        public PasswordEntry(string login, string? encryptedPassword, string salt)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Логин не может быть пустым.", nameof(login));

            Login = login;
            EncryptedPassword = encryptedPassword;
            Salt = salt;
        }

        public PasswordEntry(string login, string password, IEncryptor encryptor)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Логин не может быть пустым.", nameof(login));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Пароль не может быть пустым.", nameof(password));

            Login = login;
            Salt = GenerateSalt();
            SetEncryptedPassword(password, encryptor);
        }

        public void SetEncryptedPassword(string password, IEncryptor encryptor)
        {
            if (encryptor == null)
                throw new ArgumentNullException(nameof(encryptor), "Экземпляр шифратора не может быть null.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Пароль не может быть пустым.", nameof(password));

            EncryptedPassword = encryptor.Encrypt(password, Salt);
        }

        public string GetDecryptedPassword(IEncryptor encryptor)
        {
            return encryptor.Decrypt(EncryptedPassword, Salt);
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }

        public static PasswordEntry? Deserialize(string jsonString)
        {
            return JsonSerializer.Deserialize<PasswordEntry>(jsonString);
        }
    }
}
