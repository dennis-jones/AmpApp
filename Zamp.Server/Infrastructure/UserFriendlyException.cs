using Zamp.Shared.Models;

namespace Zamp.Server.Infrastructure;

public class UserFriendlyException(
    string code,
    string description,
    string? title = null,
    string? hint = null,
    List<ValidationError>? validationErrors = null)
    : Exception(message: description)
{
    public ExceptionModel ExceptionInfo { get; init; } = new()
    {
        Code = code,
        Description = description,
        Title = title,
        Hint = hint,
        ValidationErrors = validationErrors ?? []
    };
}

public class ValidationErrorUserFriendlyException(List<ValidationError>? validationErrors = null)
    : UserFriendlyException(
        code: "ValidationError",
        description: "This record cannot be saved until validation issues are fixed.",
        title: "Invalid Data",
        validationErrors: validationErrors
    );

public class RecordDoesNotExistOnGetException()
    : UserFriendlyException(
        code: "RecordNotFoundOnGet",
        description: "This record cannot be found. It was probably deleted by another user.",
        title: "Record Not Found"
    );

public class RecordDoesNotExistOnSaveException()
    : UserFriendlyException(
        code: "RecordNotFoundOnSave",
        description: "This record cannot be found. It was probably deleted by another user while you were editing it.",
        title: "Save Error",
        hint: "Suggestion: a) save a copy of your changes by taking a screen capture, then b) close this edit dialog box then add a new record and re-enter your information."
    );

public class RecordUpdatedByAnotherUserException(string? updatedBy = null)
    : UserFriendlyException(
        code: "RecordUpdatedByAnotherUser",
        description: $"While you were editing this record, another user ({updatedBy ?? "unknown user"}) updated it.",
        title: "Save Error",
        hint: "Suggestion: a) save a copy of your changes by taking a screen capture, then b) close this edit dialog box then re-open it to re-enter your information."
    );

// public class PermissionToViewException()
//     : UserFriendlyException(
//         code: "PermissionDeniedView",
//         description: "Permissions have not been set up to allow you to view this information.",
//         title: "Permission Denied",
//         hint: "Contact the system administrator if you require this access."
//     );
//
// public class PermissionToInsertException()
//     : UserFriendlyException(
//         code: "PermissionDeniedInsert",
//         description: "Permissions have not been set up to allow you to insert this information.",
//         title: "Permission Denied",
//         hint: "Contact the system administrator if you require this access."
//     );
//
// public class PermissionToUpdateException()
//     : UserFriendlyException(
//         code: "PermissionDeniedUpdate",
//         description: "Permissions have not been set up to allow you to update this information.",
//         title: "Permission Denied",
//         hint: "Contact the system administrator if you require this access."
//     );
//
// public class PermissionToDeleteException()
//     : UserFriendlyException(
//         code: "PermissionDeniedDelete",
//         description: "Permissions have not been set up to allow you to delete this information.",
//         title: "Permission Denied",
//         hint: "Contact the system administrator if you require this access."
//     );