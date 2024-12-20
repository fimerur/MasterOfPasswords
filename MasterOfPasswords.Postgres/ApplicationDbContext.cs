using System.Reflection;
using MasterOfPasswords.Models;
using Microsoft.EntityFrameworkCore;

namespace MasterOfPasswords.Postgres;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<DbCredential> Credentials { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load(typeof(DbCredential).Assembly.FullName!));
    }
}