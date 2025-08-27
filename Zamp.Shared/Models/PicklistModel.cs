namespace Zamp.Shared.Models;

public partial class PicklistModel
{
	public int Id { get; set; }
	public string? Txt { get; set; }
}

public partial class PicklistCriteriaModel
{
	public string PicklistType { get; set; } = default!;
	public int? IncludeId { get; set; }
	
	public bool IncludeInactive { get; set; } = false;
	public string InactivePrefix { get; set; } = "INACTIVE: ";
	public bool InactiveToEndOfList { get; set; } = true;
}
