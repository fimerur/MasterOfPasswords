using MasterOfPasswords.Authorization;

namespace MasterOfPasswords.Tests;

public class ValidatorTests
{
    private readonly IValidator _validator;

    public ValidatorTests()
    {
        _validator = new Validator();
    }

    [Theory]
    [InlineData("validPassword123", true)]
    [InlineData("anotherValid1", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void ValidateMasterPassword_ShouldReturnTrue_WhenMasterPasswordIsValid(string? masterPassword, bool shouldReturnValid)
    {
        // Act
        var result = _validator.ValidateMasterPassword(masterPassword);

        // Assert
        Assert.True(result == shouldReturnValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ValidateMasterPassword_ShouldReturnFalse_WhenMasterPasswordIsInvalid(string masterPassword)
    {
        // Act
        var result = _validator.ValidateMasterPassword(masterPassword);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("P@ssw0rd")]
    [InlineData("Password123")]
    public void ValidatePasswordFormat_ShouldReturnTrue_WhenPasswordIsValid(string password)
    {
        // Act
        var result = _validator.ValidatePasswordFormat(password);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("short")]
    [InlineData("NoDigits!")]
    [InlineData("12345678")]
    public void ValidatePasswordFormat_ShouldReturnFalse_WhenPasswordIsInvalid(string password)
    {
        // Act
        var result = _validator.ValidatePasswordFormat(password);

        // Assert
        Assert.False(result);
    }

    // Дополнительные тесты для крайних случаев
    [Theory]
    [InlineData("12345678")] // Пароль, состоящий только из цифр
    [InlineData("abcdefgh")] // Пароль, состоящий только из букв
    public void ValidatePasswordFormat_ShouldReturnFalse_WhenPasswordLacksDigitsOrLetters(string password)
    {
        // Act
        var result = _validator.ValidatePasswordFormat(password);

        // Assert
        Assert.False(result);
    }
}