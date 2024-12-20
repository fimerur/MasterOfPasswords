namespace MasterOfPasswords.Models;

public class CredentialDto(string login, string password)
{
    public string Login { get; set; } = login;

    public string Password { get; set; } = password;
}