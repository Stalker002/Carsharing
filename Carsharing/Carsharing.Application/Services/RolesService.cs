using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class RolesService : IRolesService
{
    private readonly IRolesRepository _rolesRepository;

    public RolesService(IRolesRepository rolesRepository)
    {
        _rolesRepository = rolesRepository;
    }

    public async Task<List<Role>> GetRoles()
    {
        return await _rolesRepository.GetRoles();
    }

    public async Task<int> CreateRole(Role role)
    {
        return await _rolesRepository.CreateRole(role);
    }
}