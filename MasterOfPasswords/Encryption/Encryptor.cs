using System.Security.Cryptography;
using System.Text;

namespace MasterOfPasswords.Encryption;

public class Encryptor : IEncryptor
{
    public string Encrypt(string plainText, string key)
    {
        using (RijndaelManaged rijndael = new RijndaelManaged())
        {
            rijndael.Key = Encoding.UTF8.GetBytes(key.PadRight(32));
            rijndael.IV = new byte[16];

            using (var encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV))
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public string Decrypt(string encryptedText, string key)
    {
        using (RijndaelManaged rijndael = new RijndaelManaged())
        {
            rijndael.Key = Encoding.UTF8.GetBytes(key.PadRight(32));
            rijndael.IV = new byte[16];

            using (var decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV))
            using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}