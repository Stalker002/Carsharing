using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class RolesRepository : IRolesRepository
{
    private readonly CarsharingDbContext _context;

    public RolesRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    /*public async Task<List<Role>> GetRoles()
    {
        var roleEntity = await _context.Roles
            .AsNoTracking()
            .ToListAsync();

        var roles = roleEntity
            .Select(r => Role.Create(r.Id,r.Name).roles)
            .ToList();

        return roles;
    }*/

    public Task<List<Role>> GetRoles()
    {
        throw new NotImplementedException();
    }

    public async Task<int> CreateRole(Role roles)
    {
        var roleEntity = new RoleEntity
        {
            Id = roles.Id,
            Name = roles.Name
        };

        _context.Roles.Add(roleEntity);
        await _context.SaveChangesAsync();

        return roleEntity.Id;
    }
}