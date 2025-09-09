namespace Zamp.Client.Extensions;

public static class JsRuntimeExtensions
{
    public static async Task ShowAppHelp(this IJSRuntime jsRuntime, string? stateName)
    {
        await jsRuntime.InvokeVoidAsync("showAppHelp", stateName);
    }

    public static async Task ShowAppReport(this IJSRuntime jsRuntime, string? reportName)
    {
        await jsRuntime.InvokeVoidAsync("showAppReport", reportName);
    }

    public static async Task FilterIntegerInput(this IJSRuntime jsRuntime, string id, bool allowNegativeNumber = false)
    {
        object[] args = [id, allowNegativeNumber];
        await jsRuntime.InvokeVoidAsync("filterIntegerInput", args);
    }
}
