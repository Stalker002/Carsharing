using System.Security.Claims;

namespace Carsharing.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetRequiredRoleId(this ClaimsPrincipal user)
    {
        var roleIdValue = user.FindFirst("userRoleId")?.Value;

        if (!int.TryParse(roleIdValue, out var roleId))
            throw new UnauthorizedAccessException("User role claim is missing or invalid.");

        return roleId;
    }

    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        return user.GetRequiredRoleId() == 1;
    }

    public static int GetRequiredUserId(this ClaimsPrincipal user)
    {
        var userIdValue = user.FindFirst("userId")?.Value;

        if (!int.TryParse(userIdValue, out var userId))
            throw new UnauthorizedAccessException("User ID claim is missing or invalid.");

        return userId;
    }
}