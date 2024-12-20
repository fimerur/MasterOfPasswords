using MasterOfPasswords.Models;

namespace MasterOfPasswords.Mapper;

public static class CredentialDtoMapper
{
    public static CredentialDto Map(DbCredential dbCredential)
    {
        return new CredentialDto(dbCredential.Login, dbCredential.Password);
    }
}