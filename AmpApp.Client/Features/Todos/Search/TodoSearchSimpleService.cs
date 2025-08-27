// using Amp.Case.Shared.Extensions;
//
// namespace Amp.Case.Client.Features;
//
// public class TodoSearchSimpleService(HttpClient http) : IScopedInjectable
// {
//     public TodoSearchCriteria Criteria { get; set; }
//
//     public List<TodoDto>? Rows { get; private set; }
//     public event Action? OnChange;
//
//     public async Task LoadRowsAsync()
//     {
//         Rows = await http.GetFromJsonAsync<List<TodoDto>>($"api/todo/search/simple{Criteria.ToQueryString()}");
//         NotifyStateChanged();
//     }
//
//     private void NotifyStateChanged() => OnChange?.Invoke();
// }