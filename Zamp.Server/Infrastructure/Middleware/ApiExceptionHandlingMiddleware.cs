using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Zamp.Shared.Models;

namespace Zamp.Server.Infrastructure.Middleware;

public class ApiExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ApiExceptionHandlingMiddleware> logger,
    bool returnRawErrors)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (UserFriendlyException ex)
        {
            context.Response.StatusCode = ex.ExceptionInfo.StatusCode ?? StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(ex.ExceptionInfo); // automatically sets content type to "application/json"
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception!");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ExceptionModel
            {
                Code = "General",
                StatusCode = StatusCodes.Status500InternalServerError,
                Description = "An unexpected error occurred. Please contact support.",
                Title = null,
                Details = null,
                Hint = null,
                RawMessage = returnRawErrors ? ex.Message : null,
                StackTrace = returnRawErrors ? ex.StackTrace : null
            };

            await context.Response.WriteAsJsonAsync(response); // automatically sets content type to "application/json"
        }
    }
}