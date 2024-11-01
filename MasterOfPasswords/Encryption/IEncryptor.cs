namespace MasterOfPasswords.Encryption;

public interface IEncryptor
{
    string Encrypt(string plainText, string key);
    string Decrypt(string encryptedText, string key);
}