using System.Net.Http.Json;
using Zamp.Client.Models;
using Zamp.Shared.Extensions;
using Zamp.Shared.Models.Criteria;

namespace Zamp.Client.Services;

public abstract class GridDataServiceBase<TGridCriteriaModel, TGridRowDto>(
    HttpClient httpClient,
    string url
) : GridCriteriaModel
    where TGridCriteriaModel : GridCriteriaModel
    where TGridRowDto : GridRowDto
{
    public TGridCriteriaModel Criteria { get; set; } = default!;
    public List<TGridRowDto> Rows { get; set; } = [];
    public int? TotalRowCount { get; set; }
    public TGridRowDto? SelectedRow { get; set; }
    public bool IsLoading { get; set; }
    public bool HasMoreRowsInTheDatabase { get; set; }

    public async Task LoadRowsAsync(bool firstPage = true)
    {
        try
        {
            IsLoading = true;

            // When we load page 1, any previously-selected row is forgotten (keep it simple)
            // When loading subsequent pages, select the last row to make it clear in the UI where the new rows start
            SelectedRow = firstPage ? default : Rows?.LastOrDefault();

            int offset = firstPage ? 0 : Rows?.Count ?? 0;
            var result = await httpClient.PostAsJsonAsync(url, Criteria);
            if (!result.IsSuccessStatusCode)
            {
                Rows = [];
                TotalRowCount = 0;
                return;
            }

            var searchResult = await result.Content.ReadFromJsonAsync<GridDto<TGridRowDto>>();
            TotalRowCount = searchResult?.TotalRowCount ?? 0;

            var rows = searchResult?.Rows ?? [];
            // if 51 rows are returned (assuming page size = 50) then:
            //  - remove the last row (so there are 50) and
            //  - set a flag to indicate that the db has more rows
            if (PageSize > 0 && rows.Count == PageSize + 1)
            {
                HasMoreRowsInTheDatabase = true;
                rows.RemoveAt(rows.Count - 1);
            }
            else
                HasMoreRowsInTheDatabase = false;

            Rows = firstPage ? rows : Rows?.Concat(rows).ToList();

            await OnAfterLoadRowsAsync();
        }
        catch (Exception)
        {
            Rows = [];
            TotalRowCount = 0;
            throw;
        }
        finally
        {
            // Thread.Sleep(20000);
            IsLoading = false;
        }
    }

    protected virtual async Task OnAfterLoadRowsAsync()
    {
        if (Rows != null && Rows.HasProperty("IsActive"))
        {
            foreach (var row in Rows.Where(row => !row.GetPropertyValue<bool>("IsActive")))
            {
                row.CssClass = "deemphasize";
            }
        }

        await Task.Run(() => 42);
    }
}