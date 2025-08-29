using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Zamp.Shared.Models;

namespace Zamp.Server.Infrastructure.Middleware;

public class ApiAuthorizationErrorMiddleware(
    RequestDelegate next,
    ILogger<ApiAuthorizationErrorMiddleware> logger
)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Request.Path.StartsWithSegments("/api") &&
            context.Response.StatusCode is StatusCodes.Status401Unauthorized or StatusCodes.Status403Forbidden)
        {
            string path = context.Request.Path;
            string user = (context.User.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "Anonymous") ?? string.Empty;

            logger.LogDebug("API access denied to {Path} for user {User}. Status: {StatusCode}", path, user, context.Response.StatusCode);

            if (!context.Response.HasStarted)
            {
                var response = new ExceptionModel
                {
                    Code = "General",
                    StatusCode = context.Response.StatusCode,
                    Description = context.Response.StatusCode == StatusCodes.Status401Unauthorized
                        ? "You are not logged in."
                        : "Permissions have not been set up to allow you to perform this action.",
                    Title = context.Response.StatusCode == StatusCodes.Status401Unauthorized
                        ? "Unauthorized"
                        : "Forbidden",
                    Details = null,
                    Hint = null,
                    RawMessage = null,
                    StackTrace = null
                };

                await context.Response.WriteAsJsonAsync(response); // automatically sets content type to "application/json"
            }
        }
    }
}