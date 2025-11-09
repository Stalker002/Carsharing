using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly CarsharingDbContext _context;

    public UsersRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUser()
    {
        var userEntities = await _context.Users
                               .AsNoTracking()
                               .ToListAsync();

        var users = userEntities
            .Select(u => User.Create(u.Id, u.RoleId, u.Login, u.PasswordHash).user)
            .ToList();

        return users;
    }

    public async Task<int> CreateUser(User user)
    {
        var userEntity = new DataAccess.Entites.UserEntity
                             {
                                 Id = user.Id,
                                 RoleId = user.RoleId,
                                 Login = user.Login,
                                 PasswordHash = user.PasswordHash
                             };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        return userEntity.Id;
    }

    public async Task<int> UpdateUser(int id, int roleId, string login, string passwordHash)
    {
        await _context.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.RoleId, u => roleId)
                .SetProperty(u => u.Login, u => login)
                .SetProperty(u => u.PasswordHash, u => passwordHash)
            );

        return id;
    }

    public async Task<int> DeleteUser(int id)
    {
        await _context.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}