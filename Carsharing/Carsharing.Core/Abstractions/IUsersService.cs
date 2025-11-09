using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IUsersService
{
    Task<int> CreateUser(User user);
    Task<int> DeleteUser(int id);
    Task<List<User>> GetUsers();
    Task<int> UpdateUser(int id, int roleId, string login, string passwordHash);
}