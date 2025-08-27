using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Zamp.Server.Infrastructure.Middleware;
public class ApiAuthorizationErrorMiddleware(RequestDelegate next, ILogger<ApiAuthorizationErrorMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Request.Path.StartsWithSegments("/api") &&
            context.Response.StatusCode is StatusCodes.Status401Unauthorized or StatusCodes.Status403Forbidden)
        {
            // Optional: Log the error. You can expand this later!
            logger.LogWarning("API access denied to {Path} for user {User}. Status: {StatusCode}",
                context.Request.Path,
                context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "Anonymous",
                context.Response.StatusCode);

            // Optional: Return a JSON error response (only if not already started)
            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                var error = context.Response.StatusCode == StatusCodes.Status401Unauthorized
                    ? new { error = "Unauthorized" }
                    : new { error = "Forbidden" };
                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}