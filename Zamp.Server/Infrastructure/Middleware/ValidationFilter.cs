using Microsoft.AspNetCore.Http;

namespace Zamp.Server.Infrastructure.Middleware;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var dto = context.Arguments.OfType<T>().FirstOrDefault();
        if (dto == null) return await next(context);

        var result = await validator.ValidateAsync(dto);
        if (result.IsValid) return await next(context);

        var errors = result.Errors.Select(e => new
        {
            field = e.PropertyName,
            message = e.ErrorMessage
        });
        return Results.BadRequest(new { errors });
    }
}