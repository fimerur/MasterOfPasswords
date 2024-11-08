namespace MasterOfPasswords.Tests;

public class UserTests
{
    [Fact]
    public void Authenticate_CorrectPassword_ReturnsTrue()
    {
        // Arrange
        var masterPassword = "correct_password";
        var user = new User.User(masterPassword);

        // Act
        var result = user.Authenticate("correct_password");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Authenticate_IncorrectPassword_ReturnsFalse()
    {
        // Arrange
        var masterPassword = "correct_password";
        var user = new User.User(masterPassword);

        // Act
        var result = user.Authenticate("wrong_password");

        // Assert
        Assert.False(result);
    }
}