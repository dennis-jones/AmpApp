namespace AmpApp.Features.Todo;

public class DeleteService(DeleteRepository repo, IHttpContextAccessor context) : IScopedInjectable
{
    public async Task<bool> HandleAsync(Guid id)
    {
        var username = context.HttpContext.GetLoginUserName();
        return await repo.DeleteAsync(id, username);
    }
}