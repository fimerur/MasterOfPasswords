using MasterOfPasswords.Encryption;
using MasterOfPasswords.Models;

namespace MasterOfPasswords.PasswordSetter;

public class PasswordEntry
{
    public Guid Id { get; set; }
    public string Login { get; private set; }
    public string? EncryptedPassword { get; private set; }
    public string Salt { get; private set; }

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

    public PasswordEntry(Guid id, string login, string? encryptedPassword, string salt)
    {
        Id = id;
        Login = login;
        EncryptedPassword = encryptedPassword;
        Salt = salt;
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
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }
    
    public static PasswordEntry FromDbCredential(DbCredential dbCredential)
    {
        return new PasswordEntry(dbCredential.Id, dbCredential.Login, dbCredential.Password, "");
    }

    public DbCredential ToDbCredential()
    {
        return new DbCredential
        {
            Id = this.Id,
            Login = this.Login,
            Password = this.EncryptedPassword
        };
    }
}