namespace MasterOfPasswords.Authorization;

public class Validator : IValidator
{
    public bool ValidateMasterPassword(string? masterPassword)
    {
        return !string.IsNullOrWhiteSpace(masterPassword);
    }

    public bool ValidatePasswordFormat(string password)
    {
        return password.Length >= 8 && password.Any(char.IsDigit) && password.Any(char.IsLetter);
    }
}