using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IUsersService
{
    Task<(string? Token, string? Error)> Login(string login, string password, CancellationToken cancellationToken);

    Task<List<User>> GetUsers(CancellationToken cancellationToken);

    Task<List<User>> GetPagedUsers(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetUsersCount(CancellationToken cancellationToken);

    Task<List<User>> GetUserById(int id, CancellationToken cancellationToken);

    Task<User?> GetUserByLogin(string login, CancellationToken cancellationToken);

    Task<int> CreateUser(User user, CancellationToken cancellationToken);

    Task<int> UpdateUser(int id, int? roleId, string? login, string? password, CancellationToken cancellationToken);

    Task<int> DeleteUser(int id, CancellationToken cancellationToken);
}