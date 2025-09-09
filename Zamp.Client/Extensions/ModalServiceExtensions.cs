using Blazored.Modal;
using Blazored.Modal.Services;
using Zamp.Client.Components.Modals;

namespace Zamp.Client.Extensions;

public static class ModalServiceExtensions
{
    /// <summary>
    /// Returns true is Yes is clicked, false otherwise
    /// </summary>
    public static async Task<bool> ConfirmModalAsync(this IModalService modalService, string message, string title = "Confirm")
    {
        ModalParameters parameters = new()
        {
            { "Message", message },
            { "Title", title }
        };

        var modal = modalService.Show<ConfirmModal>(string.Empty, parameters);
        var result = await modal.Result;
        return (result.Confirmed);
    }

    /// <summary>
    /// Returns the opposite logic to ConfirmAsync; useful for guard clauses (which exit a method prematurely if conditions aren't right)
    /// </summary>
    public static async Task<bool> CancelModalAsync(this IModalService modalService, string message, string title = "Confirm")
    {
        return !(await modalService.ConfirmModalAsync(message, title));
    }

    public static async Task AlertModalAsync(this IModalService modalService, string message, string title = "CITAR")
    {
        var parameters = new ModalParameters
        {
            { "Message", message },
            { "Title", title }
        };

        var modal = modalService.Show<AlertModal>(string.Empty, parameters);
        await modal.Result;
    }

    public static async Task MessageModalAsync(this IModalService modalService, string? message, string title = "CITAR")
    {
        var parameters = new ModalParameters
        {
            { "Message", message ?? "" },
            { "Title", title }
        };

        var modal = modalService.Show<MessageModal>(string.Empty, parameters);
        await modal.Result;
    }

    public static async Task ListModalAsync(
        this IModalService modalService, 
        string heading, 
        IEnumerable<string> messageList, 
        bool useOrderedList = false,
        string title = "CITAR")
    {
        var parameters = new ModalParameters
        {
            { "Title", title },
            { "Heading", heading },
            { "MessageList", messageList },
            { "UseOrderedList", useOrderedList }
        };

        var modal = modalService.Show<ListModal>(string.Empty, parameters);
        await modal.Result;
    }

    public static async Task AuditModalAsync<T>(this IModalService modalService, T record)
    {
        var parameters = new ModalParameters();
        if (record is not null)
            parameters.Add("Record", record);

        var modal = modalService.Show<AuditModal<T>>(string.Empty, parameters);
        await modal.Result;
    }
}
