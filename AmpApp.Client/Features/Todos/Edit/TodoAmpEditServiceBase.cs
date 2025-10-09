using AmpApp.Shared.Models.Todo;
using Zamp.Client.Components.EditPage;

namespace AmpApp.Client.Features.Todos.Edit;

public class TodoAmpEditServiceBase : AmpEditPageServiceBase<TodoEntity>
{
    public override async Task<Guid> InsertAsync()
    {
        throw new NotImplementedException();
    }

    public override async Task<int> UpdateAsync()
    {
        throw new NotImplementedException();
    }

    public override async Task<TodoEntity> GetRowAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}