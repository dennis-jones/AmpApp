namespace AmpApp.Features.Todo;

public class DeleteRepository(IDbConnectionFactory factory) : IScopedInjectable
{
    public async Task<bool> DeleteAsync(Guid id, string? username)
    {
        using var conn = factory.Create();
        var sql = @"DELETE FROM todo WHERE id = @Id AND created_by = @CreatedBy";
        var affected = await conn.ExecuteAsync(sql, new { Id = id, CreatedBy = username });
        return affected > 0;
    }
}