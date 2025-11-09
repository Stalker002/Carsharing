using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly CarsharingDbContext _context;
    private readonly IPasswordHasher _myPasswordHasher;

    public UsersRepository(CarsharingDbContext context, IPasswordHasher myPasswordHasher)
    {
        _context = context;
        _myPasswordHasher = myPasswordHasher;
    }

    public async Task<User> GetByLogin(string login)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == login) ?? throw new Exception();
        return User.Create(userEntity.Id, userEntity.RoleId, userEntity.Login, userEntity.PasswordHash).user;
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
        var (_, error) = User.Create(
            0,
            user.RoleId,
            user.Login,
            user.PasswordHash);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception User: {error}");

        var hashedPassword = _myPasswordHasher.Generate(user.PasswordHash);

        var userEntity = new UserEntity
                             {
                                 RoleId = user.RoleId,
                                 Login = user.Login,
                                 PasswordHash = hashedPassword
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