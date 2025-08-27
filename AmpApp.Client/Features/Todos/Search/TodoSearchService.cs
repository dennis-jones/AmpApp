// using System.Net.Http.Json;
// using AmpApp.Shared.Models.Todo;
// using Zamp.Shared.Extensions;
//
// namespace AmpApp.Client.Features.Todos.Search;
//
// public class TodoSearchSimpleService(HttpClient http) : TodoSearchService(http), IScopedInjectable
// {
//     public override string GetUrl() => $"api/todo/search/simple{Criteria.ToQueryString()}";
// }
//
// public class TodoSearchAdvancedService(HttpClient http) : TodoSearchService(http), IScopedInjectable
// {
//     public override string GetUrl() => $"api/todo/search/advanced{Criteria.ToQueryString()}";
// }
//
// public abstract class TodoSearchService(HttpClient http)
// {
//     public TodoSearchCriteria Criteria { get; set; } = new();
//     
//     public List<TodoDto>? Rows { get; private set; }
//     public event EventHandler? OnChange;
//
//     public abstract string GetUrl();
//     
//     public async Task LoadRowsAsync()
//     {
//         var url = GetUrl();
//         Rows = await http.GetFromJsonAsync<List<TodoDto>>(url);
//         NotifyStateChanged();
//     }
//
//     public void ClearAllCriteria() => Criteria.SetAllPropertiesToNull();
//     public void SetCriteriaToDefault() => Criteria.SetAllPropertiesToNull();
//
//     private void NotifyStateChanged() => OnChange?.Invoke(this, EventArgs.Empty);
// }
