using AmpApp.Shared.Models.Todo;
using Zamp.Shared.Models.Criteria;

namespace AmpApp.Features.Todo;

public class GetByCriteriaService(GetByCriteriaRepository repository, IHttpContextAccessor _) : IScopedInjectable
{
    public async Task<(IReadOnlyList<TodoDto> Rows, int TotalCount)> GetAsync(TodoGridCriteriaModel criteria)
    {
        // do pre-processing or validation of criteria here

        return await repository.QueryAsync(criteria);
    }
}