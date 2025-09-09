namespace Zamp.Client.Models;

public sealed class GridDto<TGridRowDto>
    where TGridRowDto: GridRowDto
{
    public List<TGridRowDto> Rows { get; set; } = [];
    public int TotalRowCount { get; set; }
}