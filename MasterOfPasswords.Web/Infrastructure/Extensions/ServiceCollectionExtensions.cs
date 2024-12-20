using Microsoft.EntityFrameworkCore;
using MasterOfPasswords.Postgres;

namespace MasterOfPasswords.Web.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("SQLConnectionString"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });
    }
}