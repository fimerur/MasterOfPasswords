namespace MasterOfPasswords.User;

public class User : IUser
{
    private readonly string _masterPassword;

    public User(string masterPassword)
    {
        _masterPassword = masterPassword;
    }

    public bool Authenticate(string enteredPassword)
    {
        return enteredPassword == _masterPassword;
    }
}