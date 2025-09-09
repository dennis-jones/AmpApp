using AmpApp.Shared.Models.Todo;
using Zamp.Shared.Models.Criteria;

namespace AmpApp.Features.Todo;

public class GetByCriteriaRepository(IDbConnectionFactory connectionFactory, ILogger<GetByCriteriaRepository> logger) : IScopedInjectable
{
    public async Task<(IReadOnlyList<TodoDto> Rows, int TotalCount)> QueryAsync(
        TodoGridCriteriaModel criteria,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var builder = new SqlBuilder()
                .Clear()
                .Select("*") // this is the default but just for clarity
                .From("todo")
                .AddLike(nameof(criteria.Title), criteria.Title)
                .AddLike(nameof(criteria.Description), criteria.Description)
                .AddEqual(nameof(criteria.IsComplete), criteria.IsComplete)
                .WithOrderByAndPagination(criteria);

            using var connection = connectionFactory.Create();
            var rows = (await connection.QueryAsync<TodoDto>(builder.RowsSql, builder.Parameters)).ToList();
            var totalCount = await connection.ExecuteScalarAsync<int>(builder.CountSql, builder.Parameters);
            return (rows, totalCount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database query failed for criteria: {@Criteria}", criteria);
            throw new DataAccessException("Failed to query todos from database.", ex);
        }
    }
}