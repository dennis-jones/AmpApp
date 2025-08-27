using AmpApp.Shared.Models.Todo;

namespace AmpApp.Features.Todo;

public class GetAllRepository(IDbConnectionFactory factory) : IScopedInjectable
{
    public async Task<IEnumerable<TodoEntity>> GetAllAsync(string username)
    {
        using var conn = factory.Create();
        var sql = @"SELECT id, title, description, created_by FROM todo WHERE created_by = @CreatedBy";
        return await conn.QueryAsync<TodoEntity>(sql, new { CreatedBy = username });
    }
}