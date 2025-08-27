using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Zamp.Shared.Extensions;

public static class HttpContextExtensions
{
    /// <summary>
    /// Gets the login ID (username part of email) from the current HttpContext's user claims.
    /// Returns null if not authenticated or email claim is missing.
    /// </summary>
    public static string GetLoginUserName(this HttpContext? context)
    {
        var email = context?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        return email.ExtractNameFromEmail() ?? "unknown";
    }
}