using MasterOfPasswords.Domain.Interfaces;
using MasterOfPasswords.Encryption;
using MasterOfPasswords.Models;
using Microsoft.AspNetCore.Mvc;

namespace MasterOfPasswords.Web.Controllers;

[Route("api/credentials")]
[ApiController]
[Produces("application/json")]
public class CredentialsController(ICredentialsService credentialsService, IEncryptor encryptor) : ControllerBase
{
    // POST: api/password
    [HttpPost("create")]
    public async Task AddPassword([FromBody] CredentialDto credentialDto)
    {
        await credentialsService.AddPassword(credentialDto);
    }

    // GET: api/password/{login}
    [HttpGet("{login}")]
    public async Task<CredentialDto> GetPassword(string login)
    {
        return await credentialsService.GetPassword(login);
    }

    // PUT: api/password/{login}
    [HttpPut("update")]
    public async Task UpdatePassword([FromBody]CredentialDto credentialDto)
    {
        await credentialsService.UpdatePassword(credentialDto);
    }
}