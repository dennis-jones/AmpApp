using AmpApp.Shared.Models.Todo;

namespace AmpApp.Features.Todo;

public class GetAllService (GetAllRepository repo, IHttpContextAccessor context) : IScopedInjectable
{
    public async Task<IEnumerable<TodoDto>> HandleAsync()
    {
        var username = context.HttpContext.GetLoginUserName();
        var entities = await repo.GetAllAsync(username);
        return entities.Adapt<IEnumerable<TodoDto>>();
    }
}