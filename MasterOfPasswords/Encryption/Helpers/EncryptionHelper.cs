namespace MasterOfPasswords.Encryption.Helpers;

public static class EncryptionHelper
{
    public static string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }
}