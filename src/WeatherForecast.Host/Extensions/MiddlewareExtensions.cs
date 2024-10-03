using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using WeatherForecast.Host.Common;

namespace WeatherForecast.Host.Extensions;

public static class MiddlewareExtensions
{
    public static void UseGlobalExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(err =>
        {
            err.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var feature = context.Features.Get<IExceptionHandlerFeature>();

                if (feature is not null)
                {
                    await context.Response.WriteAsJsonAsync(
                        Error.ServerError("Something went wrong.")
                            .ToProblemResult()
                            .ToProblemDetails());
                }
            });
        });
    }
}
