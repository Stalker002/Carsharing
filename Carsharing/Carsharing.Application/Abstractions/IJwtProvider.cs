using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(User user);
}