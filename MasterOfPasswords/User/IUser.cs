namespace MasterOfPasswords.User;
public interface IUser
{
    bool Authenticate(string masterPassword);
}