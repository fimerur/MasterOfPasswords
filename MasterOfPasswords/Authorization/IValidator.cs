namespace MasterOfPasswords.Authorization;

public interface IValidator
{
    bool ValidateMasterPassword(string? masterPassword);
    bool ValidatePasswordFormat(string password);
}