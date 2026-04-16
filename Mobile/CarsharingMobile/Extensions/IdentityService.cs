using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace CarsharingMobile.Extensions;

public class IdentityService
{
    public async Task<(long ProfileId, int RoleId)> GetProfileIdAsync()
    {
        var token = await SecureStorage.Default.GetAsync("tasty");
        if (string.IsNullOrEmpty(token)) return (0, 0);

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var profileIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var roleIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userRoleId")?.Value;

            return (
                long.TryParse(profileIdClaim, out var pId) ? pId : 0,
                int.TryParse(roleIdClaim, out var rId) ? rId : 0
            );
        }
        catch
        {
            return (0, 0);
        }
    }

    public bool IsTokenValid(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (jwtToken.ValidTo >= DateTime.UtcNow) return true;

            Debug.WriteLine("DEBUG: Токен просрочен");
            return false;
        }
        catch
        {
            return false;
        }
    }
}