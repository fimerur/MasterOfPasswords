using MasterOfPasswords.App;
using MasterOfPasswords.Commands;
using MasterOfPasswords.Encryption;
using MasterOfPasswords.PasswordSetter;
using MasterOfPasswords.Storage;
using MasterOfPasswords.User;
using Moq;

namespace PasswordManagerAppTests
{
    public class MasterOfPasswordsAppTests
    {
        private readonly Mock<IUser> _userMock;
        private readonly Mock<IPasswordStore> _passwordStoreMock;
        private readonly Mock<IConsoleManager> _consoleManagerMock;
        private readonly PasswordManagerApp _passwordManagerApp;

        public MasterOfPasswordsAppTests()
        {
            _userMock = new Mock<IUser>();
            _passwordStoreMock = new Mock<IPasswordStore>();
            _consoleManagerMock = new Mock<IConsoleManager>();
            _passwordManagerApp = new PasswordManagerApp(_userMock.Object, _passwordStoreMock.Object, _consoleManagerMock.Object);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldAuthenticateSuccessfully()
        {
            // Arrange
            _consoleManagerMock.Setup(c => c.ReadInput()).Returns("masterpassword");
            _userMock.Setup(u => u.Authenticate("masterpassword")).Returns(true);

            // Act
            bool result = await _passwordManagerApp.AuthenticateUserAsync();

            // Assert
            Assert.True(result);
            _consoleManagerMock.Verify(c => c.WriteInformation("Аутентификация успешна."), Times.Once);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldFailAfterMaxAttempts()
        {
            // Arrange
            _consoleManagerMock.SetupSequence(c => c.ReadInput()).Returns("wrongpassword");
            _userMock.Setup(u => u.Authenticate(It.IsAny<string>())).Returns(false);

            // Act
            bool result = await _passwordManagerApp.AuthenticateUserAsync();

            // Assert
            Assert.False(result);
            _consoleManagerMock.Verify(c => c.WriteError("Программа завершена из-за превышения количества попыток."), Times.Once);
        }

        [Fact]
        public async Task AddPasswordAsync_ShouldAddPasswordIfLoginIsUnique()
        {
            // Arrange
            _consoleManagerMock.SetupSequence(c => c.ReadInput()).Returns("uniqueLogin").Returns("password123");
            _passwordStoreMock.Setup(s => s.FindPasswordByLoginAsync("uniqueLogin")).ReturnsAsync((PasswordEntry)null);

            // Act
            await _passwordManagerApp.AddPasswordAsync();

            // Assert
            _passwordStoreMock.Verify(s => s.AddPasswordToFileAsync(It.IsAny<PasswordEntry>()), Times.Once);
            _consoleManagerMock.Verify(c => c.WriteInformation("Пароль успешно добавлен."), Times.Once);
        }

        [Fact]
        public async Task UpdatePasswordAsync_ShouldUpdatePasswordIfLoginExists()
        {
            // Arrange
            _consoleManagerMock.SetupSequence(c => c.ReadInput()).Returns("existingLogin").Returns("newPassword");
            _passwordStoreMock.Setup(s => s.FindPasswordByLoginAsync("existingLogin")).ReturnsAsync(new PasswordEntry("existingLogin", "oldPassword", new Encryptor()));

            // Act
            await _passwordManagerApp.UpdatePasswordAsync();

            // Assert
            _passwordStoreMock.Verify(s => s.UpdatePasswordInFileAsync("existingLogin", "newPassword", It.IsAny<Encryptor>()), Times.Once);
            _consoleManagerMock.Verify(c => c.WriteInformation("Пароль успешно обновлен."), Times.Once);
        }
    }
}
