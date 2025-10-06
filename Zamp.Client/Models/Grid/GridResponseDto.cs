namespace Zamp.Client.Models;

public class GridResponseDto<TGridRowDto>
    where TGridRowDto: GridRowDto
{
    public List<TGridRowDto> Rows { get; set; } = [];
    public int TotalRowCount { get; set; }
    public bool HasMoreRowsInDatabase { get; set; }
}