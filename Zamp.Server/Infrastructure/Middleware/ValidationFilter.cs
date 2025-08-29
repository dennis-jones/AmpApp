using Azure.Core.Serialization;
using Microsoft.AspNetCore.Http;
using Zamp.Server.Infrastructure;
using Zamp.Shared.Models;

namespace Zamp.Server.Infrastructure.Middleware;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var dto = context.Arguments.OfType<T>().FirstOrDefault();
        if (dto == null) return await next(context);

        var result = await validator.ValidateAsync(dto);
        if (result.IsValid) return await next(context);

        throw new ValidationErrorUserFriendlyException(
            result.Errors.Select(e => new ValidationError(propertyName: e.PropertyName, errorMessage: e.ErrorMessage)).ToList()
        );
    }
}