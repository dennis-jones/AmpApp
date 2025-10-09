namespace Zamp.Client.Components.EditPage;

public abstract class AmpEditPageServiceBase<TModel> : IScopedInjectable
{
    public TModel? Row;
    public abstract Task<Guid> InsertAsync();
    public abstract Task<int> UpdateAsync();
    public abstract Task<TModel> GetRowAsync(Guid id);
}