using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IUsersRepository
{
    Task<User> GetByLogin(string login);
    Task<List<User>> GetUser();
    Task<int> CreateUser(User user);
    Task<int> UpdateUser(int id, int? roleId, string? login, string? passwordHash);
    Task<int> DeleteUser(int id);
}