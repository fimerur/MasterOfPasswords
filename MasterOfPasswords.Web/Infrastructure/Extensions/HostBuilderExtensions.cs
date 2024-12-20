using System.Reflection;
using MasterOfPasswords.Web.Infrastructure.Swagger;
using MasterOfPasswords.Web.Infrastructure.StartupFilters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace MasterOfPasswords.Web.Infrastructure.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddInfrastructure(this IHostBuilder builder)
    {
        return builder.AddSwagger();
    }

    private static IHostBuilder AddSwagger(this IHostBuilder builder)
    {
        return builder.ConfigureServices(services =>
        {
            services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                        { Title = $"{Assembly.GetExecutingAssembly().GetName().Name}", Version = "v1" });

                options.CustomSchemaIds(x => x.FullName);

                options.EnableAnnotations();

                options.OperationFilter<HeaderOperationFilter>();
            });
        });
    }
}