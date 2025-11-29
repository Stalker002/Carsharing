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

    public async Task<string> Login(string login, string password)
    {
        var user = await _userRepository.GetByLogin(login);

        var result = _passwordHasher.Verify(password, user.PasswordHash);

        if (!result) throw new Exception("Failed to login");

        var token = _jwtProvider.GenerateToken(user);

        return token;
    }

    public async Task<List<User>> GetUsers()
    {
        return await _userRepository.GetUser();
    }

    public async Task<List<User>> GetPagedUsers(int page, int limit)
    {
        return await _userRepository.GetPagedUser(page, limit);
    }

    public async Task<int> GetUsersCount()
    {
        return await _userRepository.GetCount();
    }

    public async Task<List<User>> GetUserById(int id)
    {
        return await _userRepository.GetUserById(id);
    }

    public async Task<int> CreateUser(User user)
    {
        return await _userRepository.CreateUser(user);
    }

    public async Task<int> UpdateUser(int id, int? roleId, string? login, string? passwordHash)
    {
        return await _userRepository.UpdateUser(id, roleId, login, passwordHash);
    }

    public async Task<int> DeleteUser(int id)
    {
        return await _userRepository.DeleteUser(id);
    }

    public async Task<User> GetUserByLogin(string login)
    {
        return await _userRepository.GetByLogin(login);
    }
}