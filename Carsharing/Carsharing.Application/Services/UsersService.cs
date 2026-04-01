using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class UsersService : IUsersService
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUsersRepository _userRepository;

    public UsersService(IUsersRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<(string? Token, string? Error)> Login(string login, string password, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByLogin(login, cancellationToken);
        if (user == null)
        {
            return (null, "Пользователь с таким логином не найден");
        }

        var result = _passwordHasher.Verify(password, user.Password);
        if (!result)
        {
            return (null, "Неверный пароль");
        }

        var token = _jwtProvider.GenerateToken(user);

        return (token, null);
    }

    public async Task<List<User>> GetUsers(CancellationToken cancellationToken)
    {
        return await _userRepository.GetUser(cancellationToken);
    }

    public async Task<List<User>> GetPagedUsers(int page, int limit, CancellationToken cancellationToken)
    {
        return await _userRepository.GetPagedUser(page, limit, cancellationToken);
    }

    public async Task<int> GetUsersCount(CancellationToken cancellationToken)
    {
        return await _userRepository.GetCount(cancellationToken);
    }

    public async Task<List<User>> GetUserById(int id, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserById(id, cancellationToken);
    }

    public async Task<User?> GetUserByLogin(string login, CancellationToken cancellationToken)
    {
        return await _userRepository.GetByLogin(login, cancellationToken);
    }

    public async Task<int> CreateUser(User user, CancellationToken cancellationToken)
    {
        var (validatedUser, error) = User.Create(0, user.RoleId, user.Login, user.Password);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception User: {error}");

        var hashedPassword = _passwordHasher.Generate(validatedUser.Password);
        var userToCreate = User.Restore(0, validatedUser.RoleId, validatedUser.Login, hashedPassword);

        return await _userRepository.CreateUser(userToCreate, cancellationToken);
    }

    public async Task<int> UpdateUser(int id, int? roleId, string? login, string? password, CancellationToken cancellationToken)
    {
        var existingUser = (await _userRepository.GetUserById(id, cancellationToken)).SingleOrDefault()
            ?? throw new Exception("User not found");

        var nextRoleId = roleId ?? existingUser.RoleId;
        var nextLogin = string.IsNullOrWhiteSpace(login) ? existingUser.Login : login;

        if (string.IsNullOrWhiteSpace(password))
        {
            User.Restore(id, nextRoleId, nextLogin, existingUser.Password);
            return await _userRepository.UpdateUser(id, nextRoleId, nextLogin, null, cancellationToken);
        }

        var (validatedUser, error) = User.Create(id, nextRoleId, nextLogin, password);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception User: {error}");

        var passwordHash = _passwordHasher.Generate(validatedUser.Password);

        return await _userRepository.UpdateUser(id, validatedUser.RoleId, validatedUser.Login, passwordHash, cancellationToken);
    }

    public async Task<int> DeleteUser(int id, CancellationToken cancellationToken)
    {
        return await _userRepository.DeleteUser(id, cancellationToken);
    }
}
