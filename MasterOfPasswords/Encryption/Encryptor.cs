using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MasterOfPasswords.Encryption;

public class Encryptor : IEncryptor
{
    private IEncryptor _encryptorImplementation;

    public string Encrypt(string plainText, string key)
    {
        using (var rijndael = new RijndaelManaged())
        {
            rijndael.GenerateIV();
                
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            rijndael.Key = GenerateKey(key, salt);
            rijndael.Mode = CipherMode.CBC;
            rijndael.Padding = PaddingMode.PKCS7;

            using (var encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV))
            using (var ms = new MemoryStream())
            {
                ms.Write(salt, 0, salt.Length);
                ms.Write(rijndael.IV, 0, rijndael.IV.Length);

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
        byte[] fullCipher = Convert.FromBase64String(encryptedText);

        using (var rijndael = new RijndaelManaged())
        {
            byte[] salt = new byte[16];
            byte[] iv = new byte[rijndael.BlockSize / 8];
            
            Array.Copy(fullCipher, 0, salt, 0, salt.Length);
            Array.Copy(fullCipher, salt.Length, iv, 0, iv.Length);

            rijndael.Key = GenerateKey(key, salt);
            rijndael.IV = iv;
            rijndael.Mode = CipherMode.CBC;
            rijndael.Padding = PaddingMode.PKCS7;

            using (var decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV))
            using (var ms = new MemoryStream(fullCipher, salt.Length + iv.Length, fullCipher.Length - salt.Length - iv.Length))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }

    private byte[] GenerateKey(string key, byte[] salt)
    {
        using (var deriveBytes = new Rfc2898DeriveBytes(key, salt, 10000))
        {
            return deriveBytes.GetBytes(16);
        }
    }
}