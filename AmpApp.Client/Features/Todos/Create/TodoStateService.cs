using System.Net.Http.Json;
using AmpApp.Shared.Models.Todo;

namespace AmpApp.Client.Features.Todos.Create;

public class TodoStateService(HttpClient httpClient) : IScopedInjectable
{
    public List<TodoDto> Todos { get; private set; } = [];

    public async Task RefreshTodosAsync()
    {
        // var result = await httpClient.PostAsJsonAsync("/api/todo/search", new TodoSearchCriteria());
        // if (!result.IsSuccessStatusCode)
        //     return;
        //
        // var searchResult = await result.Content.ReadFromJsonAsync<TodoSearchResult>();
        // Todos = searchResult?.Rows ?? [];
    }

    public async Task<bool> AddTodoAsync(CreateTodoDto dto)
    {
        var response = await httpClient.PostAsJsonAsync("/api/todo", dto);
        if (!response.IsSuccessStatusCode) return false;

        // Optionally, could re-fetch or append new item
        await RefreshTodosAsync();
        return true;
    }
}