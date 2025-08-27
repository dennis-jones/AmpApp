using AmpApp.Shared.Models.Todo;

namespace AmpApp.Features.Todo;

public class GetByIdRepository(IDbConnectionFactory factory) : IScopedInjectable
{
    public async Task<TodoEntity?> GetByIdAsync(Guid id, string username)
    {
        using var conn = factory.Create();
        var sql = "SELECT id, title, description, created_by FROM todo WHERE id = @Id AND created_by = @CreatedBy";
        return await conn.QuerySingleOrDefaultAsync<TodoEntity>(sql, new { Id = id, CreatedBy = username });
    }
}