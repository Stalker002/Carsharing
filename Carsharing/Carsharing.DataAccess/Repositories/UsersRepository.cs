using Carsharing.Application.Abstractions;
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

    public async Task<User?> GetByLogin(string login)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == login);

        if (userEntity == null) return null;

        var (user, error) = User.Create(
            userEntity.Id,
            userEntity.RoleId,
            userEntity.Login,
            userEntity.Password
        );

        return user;
    }

    public async Task<List<User>> GetUser()
    {
        var userEntities = await _context.Users
            .OrderBy(u => u.Id)
            .AsNoTracking()
            .ToListAsync();

        var users = userEntities
            .Select(u => User.Create(u.Id, u.RoleId, u.Login, u.Password).user)
            .ToList();

        return users;
    }

    public async Task<List<User>> GetPagedUser(int page, int limit)
    {
        var userEntities = await _context.Users
            .AsNoTracking()
            .OrderBy(u => u.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        var users = userEntities
            .Select(u => User.Create(
                u.Id,
                u.RoleId,
                u.Login,
                u.Password).user)
            .ToList();

        return users;
    }

    public async Task<int> GetCount()
    {
        return await _context.Users.CountAsync();
    }

    public async Task<List<User>> GetUserById(int id)
    {
        var userEntities = await _context.Users
            .Where(u => u.Id == id)
            .AsNoTracking()
            .ToListAsync();

        var users = userEntities
            .Select(u => User.Create(u.Id, u.RoleId, u.Login, u.Password).user)
            .ToList();

        return users;
    }

    public async Task<int> CreateUser(User user)
    {
        var (_, error) = User.Create(
            0,
            user.RoleId,
            user.Login,
            user.Password);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception User: {error}");

        var hashedPassword = _myPasswordHasher.Generate(user.Password);

        var userEntity = new UserEntity
        {
            RoleId = user.RoleId,
            Login = user.Login,
            Password = hashedPassword
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        return userEntity.Id;
    }

    public async Task<int> UpdateUser(int id, int? roleId, string? login, string? password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
                   ?? throw new Exception("User not found");

        if (roleId.HasValue)
            user.RoleId = roleId.Value;

        if (!string.IsNullOrWhiteSpace(login))
            user.Login = login;

        if (!string.IsNullOrWhiteSpace(password))
            user.Password = password;

        var (_, error) = User.Create(
            0,
            user.RoleId,
            user.Login,
            user.Password);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception User: {error}");

        user.Password = _myPasswordHasher.Generate(user.Password);

        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task<int> DeleteUser(int id)
    {
        await _context.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}