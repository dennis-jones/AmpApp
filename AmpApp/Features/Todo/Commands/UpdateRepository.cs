using AmpApp.Shared.Models.Todo;

namespace AmpApp.Features.Todo;

public class UpdateRepository(IDbConnectionFactory factory) : IScopedInjectable
{
    public async Task<TodoEntity?> UpdateAsync(TodoEntity entity)
    {
        using var conn = factory.Create();
        var sql = @"UPDATE todo
                    SET title = @Title, description = @Description
                    WHERE id = @Id AND created_by = @CreatedBy
                    RETURNING id, title, description, created_by";
        return await conn.QuerySingleOrDefaultAsync<TodoEntity>(sql, entity);
    }
}