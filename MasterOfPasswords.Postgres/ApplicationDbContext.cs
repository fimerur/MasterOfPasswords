using System.Reflection;
using MasterOfPasswords.Models;
using Microsoft.EntityFrameworkCore;

namespace MasterOfPasswords.Postgres;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<DbCredential> Credentials { get; set; } = null!;

    public ApplicationDbContext()
    {
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=passwordsDb;username=postgres;password=password");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load(typeof(DbCredential).Assembly.FullName!));
    }
}