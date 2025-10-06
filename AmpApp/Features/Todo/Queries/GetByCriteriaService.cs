using AmpApp.Shared.Models.Todo;
using Zamp.Client.Models;

namespace AmpApp.Features.Todo;

public class GetByCriteriaService(GetByCriteriaRepository repository, IHttpContextAccessor _) : IScopedInjectable
{
    public async Task<GridResponseDto<TodoGridRowDto>> GetAsync(TodoGridCriteriaModel criteria)
    {
        // do pre-processing or validation of criteria here

        return await repository.QueryAsync(criteria);
    }
}