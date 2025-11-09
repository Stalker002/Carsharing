using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IRolesRepository
{
    Task<List<Role>> GetRoles();
    Task<int> CreateRole(Role roles);
}