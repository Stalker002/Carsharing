using Carsharing.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class UserRepository
{
    private readonly CarsharingDbContext _context;

    public UserRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUser()
    {
        var userEntities = await _context.Users
            .AsNoTracking()
            .ToListAsync();
        var users = userEntities
            .Select(u => User.Create(u.Id,u.RoleId,u.Login,u.PasswordHash).User)
            .ToList();
        return users;
    }
}