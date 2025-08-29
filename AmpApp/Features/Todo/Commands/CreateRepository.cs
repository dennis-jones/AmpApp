using AmpApp.Shared.Models.Todo;
using Zamp.Server.Infrastructure;

namespace AmpApp.Features.Todo;

public class CreateRepository(IDbConnectionFactory factory) : IScopedInjectable
{
    public virtual async Task<Guid> CreateAsync(TodoEntity entity)
    {
        using var conn = factory.Create();
        const string sql =
            """
            INSERT INTO todo (id, title, description, created_by, updated_by)
            VALUES (@Id, @Title, @Description, @CreatedBy, @UpdatedBy)
            RETURNING id
            """;
        return await conn.QuerySingleAsync<Guid>(sql, entity);
    }
}