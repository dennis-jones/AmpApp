using System.Net.Http.Json;
using AmpApp.Shared.Models.Todo;
using Zamp.Client.Services;

namespace AmpApp.Client.Features.Todos.Search;

public class TodoGridDataService : GridDataServiceBase<TodoGridCriteriaModel, TodoGridRowDto>, IScopedInjectable
{
    public TodoGridDataService(HttpClient httpClient) : base(httpClient, "/api/todo/search")
    {
        Criteria = new TodoGridCriteriaModel();
    }
}

