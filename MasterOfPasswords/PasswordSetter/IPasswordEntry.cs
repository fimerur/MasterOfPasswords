using MasterOfPasswords.Encryption;

namespace MasterOfPasswords.PasswordSetter;

public interface IPasswordEntry
{
    void SetEncryptedPassword(string password, IEncryptor encryptor);
    string GetDecryptedPassword(IEncryptor encryptor);
}