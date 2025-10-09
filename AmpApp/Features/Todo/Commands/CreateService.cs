using AmpApp.Shared.Models.Todo;

namespace AmpApp.Features.Todo;

public class CreateService(CreateRepository repo, IHttpContextAccessor context) : IScopedInjectable
{
    public async Task<IdDto> HandleAsync(TodoEntity row)
    {
        var entity = row.Adapt<TodoEntity>();
        entity.Id = Guid.CreateVersion7();
        var loginUserName = context.HttpContext.GetLoginUserName();
        entity.CreatedBy = loginUserName;
        entity.UpdatedBy = loginUserName;

        var id = await repo.CreateAsync(entity);

        return new IdDto { Id = id };
    }
}