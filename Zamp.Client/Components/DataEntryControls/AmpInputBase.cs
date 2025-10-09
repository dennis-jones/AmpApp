namespace Zamp.Client.Components.DataEntryControls;

public class AmpInputBase<TValue> : ComponentBase, IDisposable
{
    private FieldIdentifier _fieldIdentifier;

    protected override void OnInitialized()
    {
        if (For is not null)
        {
            _fieldIdentifier = FieldIdentifier.Create(For);
            EditContext.OnValidationStateChanged += HandleValidationStateChanged;
        }
        base.OnInitialized();
    }

    public void Dispose()
    {
        if (For is not null)
        {
            EditContext.OnValidationStateChanged -= HandleValidationStateChanged;
        }
        GC.SuppressFinalize(this);
    }

    [CascadingParameter] protected EditContext EditContext { get; set; } = default!;
    [Parameter] public Expression<Func<TValue>>? For { get; set; } // For is the model property to be bound to this control (For wouldn't be my choice for the name but is used to be consistent with Blazor's input components)

    protected List<string> ValidationMessages 
        => For == null ? [] : EditContext.GetValidationMessages(_fieldIdentifier).ToList();

    private void HandleValidationStateChanged(object? o, ValidationStateChangedEventArgs args) => StateHasChanged();

    protected string GetValidationMessages()
    {
        StringBuilder sb = new();
        foreach (var message in ValidationMessages)
        {
            sb.Append($" | {message}");
        }
        return sb.Length == 0 ? string.Empty : sb.ToString()[3..];
    }
}
