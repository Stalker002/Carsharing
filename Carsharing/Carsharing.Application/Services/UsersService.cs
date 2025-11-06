using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _userRepository;

    public UsersService(IUsersRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<User>> GetUsers()
    {
        return await _userRepository.GetUser();
    }

    public async Task<int> CreateUser(User user)
    {
        return await _userRepository.CreateUser(user);
    }

    public async Task<int> UpdateUser(int id, int roleId, string login, string passwordHash)
    {
        return await _userRepository.UpdateUser(id, roleId, login, passwordHash);
    }

    public async Task<int> DeleteUser(int id)
    {
        return await _userRepository.DeleteUser(id);
    }
}