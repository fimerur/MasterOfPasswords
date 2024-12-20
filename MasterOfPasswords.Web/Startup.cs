using System.Text.Json.Serialization;
using MasterOfPasswords.Domain;
using MasterOfPasswords.Domain.Interfaces;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using MasterOfPasswords.Encryption;
using MasterOfPasswords.Postgres;
using MasterOfPasswords.Web.Infrastructure.Extensions;
using MasterOfPasswords.Web.Infrastructure.Middlewares;

namespace MasterOfPasswords.Web;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        
        services.AddMvc();
        
        services.AddInfrastructureDbContext(Configuration);
        
        services.AddSingleton<IEncryptor, Encryptor>();
        services.AddSingleton<ICredentialsService, CredentialsService>(); //?


        services.AddHttpContextAccessor();
        services.AddMemoryCache();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseHsts();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            app.UseHttpsRedirection();
        }
        
        UpdateDatabase(app);
        
        app.UseMiddleware<GlobalExceptionMiddleware>();
        
        app.UseRouting();
        
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    private void UpdateDatabase(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        
        context?.Database.Migrate();
    }
}
