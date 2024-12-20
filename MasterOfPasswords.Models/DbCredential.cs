using System.ComponentModel.DataAnnotations;

namespace MasterOfPasswords.Models;

public class DbCredential
{
    [Key]
    public Guid Id { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required string Salt { get; set; }
}

