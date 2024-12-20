using System.Text.Json;

namespace MasterOfPasswords.Web.Infrastructure.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(
                new
                {
                    ErrorMessage = ex.Message,
                    Status = httpContext.Response.StatusCode
                }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }
    }
}