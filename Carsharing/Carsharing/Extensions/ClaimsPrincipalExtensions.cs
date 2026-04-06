using System.Security.Claims;

namespace Carsharing.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetRequiredUserId(this ClaimsPrincipal user)
    {
        var userIdValue = user.FindFirst("userId")?.Value;

        if (!int.TryParse(userIdValue, out var userId))
            throw new UnauthorizedAccessException("User ID claim is missing or invalid.");

        return userId;
    }
}
