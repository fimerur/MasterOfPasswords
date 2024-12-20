using MasterOfPasswords.Domain.Interfaces;
using MasterOfPasswords.Encryption;
using MasterOfPasswords.Encryption.Helpers;
using MasterOfPasswords.Mapper;
using MasterOfPasswords.Models;
using MasterOfPasswords.Postgres;
using Microsoft.EntityFrameworkCore;

namespace MasterOfPasswords.Domain;

public class CredentialsService(IEncryptor encryptor, ApplicationDbContext dbContext) : ICredentialsService
{
    public async Task AddPassword(CredentialDto credentialDto)
    {
        if (string.IsNullOrWhiteSpace(credentialDto.Login))
            throw new ArgumentException("Логин не может быть пустым.", nameof(credentialDto.Login));

        if (string.IsNullOrWhiteSpace(credentialDto.Password))
            throw new ArgumentException("Пароль не может быть пустым.", nameof(credentialDto.Password));

        var salt = EncryptionHelper.GenerateSalt();
        var encryptedPassword = encryptor.Encrypt(credentialDto.Password, salt);

        var dbCredential = new DbCredential()
        {
            Id = Guid.NewGuid(),
            Login = credentialDto.Login,
            Password = encryptedPassword,
            Salt = salt
        };

        await dbContext.Credentials.AddAsync(dbCredential);
        await dbContext.SaveChangesAsync();
    }

    public async Task<CredentialDto> GetPassword(string login)
    {
        var dbCredential = await dbContext.Credentials
            .Where(c => c.Login == login)
            .FirstOrDefaultAsync();

        if (dbCredential == null)
            throw new Exception("Логин не может быть пустым.");

        dbCredential.Password = encryptor.Decrypt(dbCredential.Password, dbCredential.Salt);
        return CredentialDtoMapper.Map(dbCredential);
    }

    public async Task UpdatePassword(CredentialDto updatedCredentialDto)
    {
        if (string.IsNullOrWhiteSpace(updatedCredentialDto.Login))
            throw new ArgumentException("Логин не может быть пустым.", nameof(updatedCredentialDto.Login));

        if (string.IsNullOrWhiteSpace(updatedCredentialDto.Password))
            throw new ArgumentException("Пароль не может быть пустым.", nameof(updatedCredentialDto.Password));

        var dbCredential = await dbContext.Credentials
            .Where(c => c.Login == updatedCredentialDto.Login)
            .FirstOrDefaultAsync();

        if (dbCredential == null)
            throw new Exception("Логин не может быть пустым");

        var salt = EncryptionHelper.GenerateSalt();
        var encryptedPassword = encryptor.Encrypt(updatedCredentialDto.Password, salt);

        dbCredential.Login = updatedCredentialDto.Login;
        dbCredential.Password = encryptedPassword;
        dbCredential.Salt = salt;

        await dbContext.SaveChangesAsync();
    }
}