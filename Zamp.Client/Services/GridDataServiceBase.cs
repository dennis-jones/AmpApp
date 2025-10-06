using System.Net.Http.Json;
using Zamp.Client.Models;
using Zamp.Shared.Extensions;
using Zamp.Shared.Models.Criteria;

namespace Zamp.Client.Services;

public abstract class GridDataServiceBase<TGridCriteriaModel, TGridRowDto>(
    HttpClient httpClient,
    string endpoint
)
    where TGridCriteriaModel : GridCriteriaModel
    where TGridRowDto : GridRowDto
{
    #region Properties

    public TGridCriteriaModel Criteria { get; set; } = default!;
    public List<TGridRowDto> Rows { get; set; } = [];
    public int TotalRowCount { get; set; }
    public bool HasMoreRowsInDatabase { get; set; }
    public bool IsLoading { get; set; }

    #endregion
    
    public async Task LoadRowsAsync(bool firstPage = true)
    {
        try
        {
            IsLoading = true;
            
            Criteria.Offset = firstPage ? 0 : Rows.Count;
            var response = await httpClient.PostAsJsonAsync(endpoint, Criteria);
            if (!response.IsSuccessStatusCode)
            {
                Rows = [];
                TotalRowCount = 0;
                HasMoreRowsInDatabase = false;
                return;
            }

            var content = await response.Content.ReadFromJsonAsync<GridResponseDto<TGridRowDto>>() 
                               ?? new GridResponseDto<TGridRowDto>();
            
            // When loading subsequent pages (not first page), select the last row to make it clear in the UI where the new rows start
            if (!firstPage && Rows.Count > 0)
            {
                foreach (var row in Rows.Where(x => x.IsSelected))
                    row.IsSelected = false;
                Rows.Last().IsSelected = true;
            }
            
            Rows = firstPage 
                ? content.Rows 
                : Rows.Concat(content.Rows).DistinctBy(row => row.Id).ToList();
            TotalRowCount = content.TotalRowCount;
            HasMoreRowsInDatabase = content.HasMoreRowsInDatabase;

            await OnAfterLoadRowsAsync();
        }
        catch (Exception)
        {
            Rows = [];
            TotalRowCount = 0;
            HasMoreRowsInDatabase = false;
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
        if (Rows.HasProperty("IsActive"))
        {
            foreach (var row in Rows)
            {
                row.CssClass = row.GetPropertyValue<bool>("IsActive") 
                    ? "deemphasize" 
                    : string.Empty;
            }
        }

        await Task.Run(() => 42);
    }

    public TGridRowDto? GetSelectedRow() => Rows.FirstOrDefault(r => r.IsSelected);
    
    public Guid? GetSelectedRowId() => GetSelectedRow()?.Id;

}