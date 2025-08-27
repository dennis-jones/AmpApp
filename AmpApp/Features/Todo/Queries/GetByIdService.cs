using AmpApp.Shared.Models.Todo;

namespace AmpApp.Features.Todo;

public class GetByIdService(GetByIdRepository repo, IHttpContextAccessor context) : IScopedInjectable
{
    public async Task<TodoDto?> HandleAsync(Guid id)
    {
        var username = context.HttpContext.GetLoginUserName();
        var entity = await repo.GetByIdAsync(id, username);
        return entity?.Adapt<TodoDto>();
    }
}