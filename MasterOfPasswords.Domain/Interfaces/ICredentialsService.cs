using MasterOfPasswords.Models;

namespace MasterOfPasswords.Domain.Interfaces;

public interface ICredentialsService
{
    Task AddPassword(CredentialDto credentialDto);

    Task<CredentialDto> GetPassword(string login);
    
    Task UpdatePassword(CredentialDto updatedCredentialDto);
}