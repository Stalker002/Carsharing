using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly CarsharingDbContext _context;

    public UsersRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByLogin(string login, CancellationToken cancellationToken)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);

        if (userEntity == null) return null;

        return User.Restore(
            userEntity.Id,
            userEntity.RoleId,
            userEntity.Login,
            userEntity.Password
        );
    }

    public async Task<List<User>> GetUser(CancellationToken cancellationToken)
    {
        var userEntities = await _context.Users
            .OrderBy(u => u.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var users = userEntities
            .Select(u => User.Restore(u.Id, u.RoleId, u.Login, u.Password))
            .ToList();

        return users;
    }

    public async Task<List<User>> GetPagedUser(int page, int limit, CancellationToken cancellationToken)
    {
        var userEntities = await _context.Users
            .AsNoTracking()
            .OrderBy(u => u.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var users = userEntities
            .Select(u => User.Restore(u.Id, u.RoleId, u.Login, u.Password))
            .ToList();

        return users;
    }

    public async Task<int> GetCount(CancellationToken cancellationToken)
    {
        return await _context.Users.CountAsync(cancellationToken);
    }

    public async Task<List<User>> GetUserById(int id, CancellationToken cancellationToken)
    {
        var userEntities = await _context.Users
            .Where(u => u.Id == id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var users = userEntities
            .Select(u => User.Restore(u.Id, u.RoleId, u.Login, u.Password))
            .ToList();

        return users;
    }

    public async Task<int> CreateUser(User user, CancellationToken cancellationToken)
    {
        var userEntity = new UserEntity
        {
            RoleId = user.RoleId,
            Login = user.Login,
            Password = user.Password
        };

        await _context.Users.AddAsync(userEntity, cancellationToken);
        await _context.SaveChangesAsync();

        return userEntity.Id;
    }

    public async Task<int> UpdateUser(int id, int? roleId, string? login, string? passwordHash, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
                   ?? throw new Exception("User not found");

        if (roleId.HasValue)
            user.RoleId = roleId.Value;

        if (!string.IsNullOrWhiteSpace(login))
            user.Login = login;

        if (!string.IsNullOrWhiteSpace(passwordHash))
            user.Password = passwordHash;

        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task<int> DeleteUser(int id, CancellationToken cancellationToken)
    {
        await _context.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return id;
    }
}
