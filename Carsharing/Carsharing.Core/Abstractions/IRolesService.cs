using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IRolesService
{
    Task<List<Role>> GetRoles();
    Task<int> CreateRole(Role role);
}