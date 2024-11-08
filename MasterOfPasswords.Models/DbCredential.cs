using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterOfPasswords.Models;

public class DbCredential
{
    [Key]
    public Guid Id { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
}

