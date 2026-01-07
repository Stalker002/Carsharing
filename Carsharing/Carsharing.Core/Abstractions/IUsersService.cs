using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IUsersService
{
    Task<(string? Token, string? Error)> Login(string login, string password);
    Task<List<User>> GetUsers();
    Task<List<User>> GetPagedUsers(int page, int limit);
    Task<int> GetUsersCount();
    Task<List<User>> GetUserById(int id);
    Task<User?> GetUserByLogin(string login);
    Task<int> CreateUser(User user);
    Task<int> UpdateUser(int id, int? roleId, string? login, string? passwordHash);
    Task<int> DeleteUser(int id);
}