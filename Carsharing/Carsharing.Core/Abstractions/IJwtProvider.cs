using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(User user);
}