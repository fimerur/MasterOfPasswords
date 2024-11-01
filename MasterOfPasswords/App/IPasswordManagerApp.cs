namespace MasterOfPasswords.App
{
    public interface IPasswordManagerApp
    {
        Task<bool> AuthenticateUserAsync();
        Task StartMenuAsync();
        Task AddPasswordAsync();
        Task FindPasswordAsync();
        Task UpdatePasswordAsync();
    }
}