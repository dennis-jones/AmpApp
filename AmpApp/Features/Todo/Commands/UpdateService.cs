using AmpApp.Shared.Models.Todo;

namespace AmpApp.Features.Todo;

public class UpdateService(UpdateRepository repo, IHttpContextAccessor context) : IScopedInjectable
{
    public async Task<TodoDto?> HandleAsync(Guid id, TodoDto dto)
    {
        // Validation.Validate(dto);

        var username = context.HttpContext.GetLoginUserName();
        var entity = dto.Adapt<TodoEntity>();
        entity.UpdatedBy = username;

        var updated = await repo.UpdateAsync(entity);

        return updated?.Adapt<TodoDto>();
    }
}