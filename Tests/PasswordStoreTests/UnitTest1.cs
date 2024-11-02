using MasterOfPasswords.Encryption;
using MasterOfPasswords.PasswordSetter;
using MasterOfPasswords.Storage;
using Moq;

namespace PasswordStoreTests;

public class PasswordStoreTests
{
    private readonly Mock<IDataStorage> _mockStorage;
    private readonly Mock<IEncryptor> _mockEncryptor;

    public PasswordStoreTests()
    {
        _mockStorage = new Mock<IDataStorage>();
        _mockEncryptor = new Mock<IEncryptor>();
    }

    [Fact]
    public async Task AddPasswordToFileAsync_ShouldAddPasswordEntry()
    {
        // Arrange
        var passwordStore = new PasswordStore(_mockStorage.Object);
        var entry = new PasswordEntry("test_login", "test_password", _mockEncryptor.Object);

        _mockStorage.Setup(s => s.LoadAsync())
            .ReturnsAsync(new List<PasswordEntry>());

        // Act
        await passwordStore.AddPasswordToFileAsync(entry);
    }

    [Fact]
    public async Task FindPasswordByLoginAsync_ShouldReturnCorrectEntry_WhenEntryExists()
    {
        // Arrange
        var entry = new PasswordEntry("test_login", "encrypted_password", "salt");
        var passwordStore = new PasswordStore(_mockStorage.Object);

        _mockStorage.Setup(s => s.LoadAsync())
            .ReturnsAsync(new List<PasswordEntry> { entry });

        // Act
        var result = await passwordStore.FindPasswordByLoginAsync("test_login");
        
    }

    [Fact]
    public async Task FindPasswordByLoginAsync_ShouldReturnNull_WhenEntryDoesNotExist()
    {
        // Arrange
        var passwordStore = new PasswordStore(_mockStorage.Object);

        _mockStorage.Setup(s => s.LoadAsync())
            .ReturnsAsync(new List<PasswordEntry>());

        // Act
        var result = await passwordStore.FindPasswordByLoginAsync("non_existent_login");
        
    }

    [Fact]
    public async Task UpdatePasswordInFileAsync_ShouldUpdatePassword_WhenEntryExists()
    {
        // Arrange
        var entry = new PasswordEntry("test_login", "old_encrypted_password", "salt");
        var passwordStore = new PasswordStore(_mockStorage.Object);

        _mockEncryptor.Setup(e => e.Encrypt("new_password", entry.Salt)).Returns("new_encrypted_password");

        _mockStorage.Setup(s => s.LoadAsync())
            .ReturnsAsync(new List<PasswordEntry> { entry });

        // Act
        await passwordStore.UpdatePasswordInFileAsync("test_login", "new_password", _mockEncryptor.Object);

        // Assert
    }

    [Fact]
    public async Task UpdatePasswordInFileAsync_ShouldDoNothing_WhenEntryDoesNotExist()
    {
        // Arrange
        var passwordStore = new PasswordStore(_mockStorage.Object);

        _mockStorage.Setup(s => s.LoadAsync())
            .ReturnsAsync(new List<PasswordEntry>());

        // Act
        await passwordStore.UpdatePasswordInFileAsync("non_existent_login", "new_password", _mockEncryptor.Object);
        
    }
}