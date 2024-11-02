using MasterOfPasswords.Encryption;

namespace EncryptorTests;

public class EncryptorTests
{
    private readonly IEncryptor _encryptor;

    public EncryptorTests()
    {
        _encryptor = new Encryptor();
    }

    [Theory]
    [InlineData("Hello, World!", "mysecurekey123456")]
    [InlineData("Test123", "anotherkey123456")]
    public void Encrypt_ShouldReturnEncryptedText_WhenGivenPlainTextAndKey(string plainText, string key)
    {
        // Act
        var encryptedText = _encryptor.Encrypt(plainText, key);

        // Assert
        Assert.NotNull(encryptedText);
        Assert.NotEqual(plainText, encryptedText); // Убедитесь, что зашифрованный текст отличается от исходного
        Assert.True(encryptedText.Length > 0); // Убедитесь, что зашифрованный текст не пустой
    }

    [Theory]
    [InlineData("Hello, World!", "mysecurekey123456")]
    [InlineData("Test123", "anotherkey123456")]
    public void Decrypt_ShouldReturnOriginalText_WhenGivenEncryptedTextAndKey(string plainText, string key)
    {
        // Act
        var encryptedText = _encryptor.Encrypt(plainText, key); // Шифруем текст
        var decryptedText = _encryptor.Decrypt(encryptedText, key); // Дешифруем текст

        // Assert
        Assert.Equal(plainText, decryptedText); // Проверяем, что дешифрованный текст равен исходному
    }

    [Fact]
    public void Decrypt_ShouldThrowFormatException_WhenGivenInvalidBase64String()
    {
        // Arrange
        string invalidBase64 = "invalidBase64String";
        string key = "mysecurekey123456";

        // Act & Assert
        Assert.Throws<FormatException>(() => _encryptor.Decrypt(invalidBase64, key)); // Проверяем, что выбрасывается исключение
    }

    [Theory]
    [InlineData("Hello, World!", "mysecurekey123456")]
    public void EncryptAndDecrypt_ShouldPreserveData_WhenUsingSameKey(string plainText, string key)
    {
        // Act
        var encryptedText = _encryptor.Encrypt(plainText, key); // Шифруем текст
        var decryptedText = _encryptor.Decrypt(encryptedText, key); // Дешифруем текст

        // Assert
        Assert.Equal(plainText, decryptedText); // Проверяем, что дешифрованный текст равен исходному
    }
}