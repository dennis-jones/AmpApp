using AmpApp.Shared.Models.Todo;
using Zamp.Client.Models;
using Zamp.Shared.Models.Criteria;

namespace AmpApp.Features.Todo;

public class GetByCriteriaRepository(IDbConnectionFactory connectionFactory, ILogger<GetByCriteriaRepository> logger) : IScopedInjectable
{
    public async Task<GridResponseDto<TodoGridRowDto>> QueryAsync(
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

            var response = new GridResponseDto<TodoGridRowDto>();
            using var connection = connectionFactory.Create();
            response.Rows = (await connection.QueryAsync<TodoGridRowDto>(builder.RowsSql, builder.Parameters)).ToList();
            if (criteria.PageSize == 0 || criteria.DisablePagination || response.Rows.Count <= criteria.PageSize)
                response.HasMoreRowsInDatabase = false;
            else
            {
                response.Rows.RemoveAt(response.Rows.Count - 1);
                response.HasMoreRowsInDatabase = true;
            }

            response.TotalRowCount = await connection.ExecuteScalarAsync<int>(builder.CountSql, builder.Parameters);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database query failed for criteria: {@Criteria}", criteria);
            throw new DataAccessException("Failed to query todos from database.", ex);
        }
    }
}