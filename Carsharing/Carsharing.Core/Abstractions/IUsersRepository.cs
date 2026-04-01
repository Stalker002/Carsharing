using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IUsersRepository
{
    Task<User?> GetByLogin(string login, CancellationToken cancellationToken);

    Task<List<User>> GetUser(CancellationToken cancellationToken);

    Task<List<User>> GetPagedUser(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCount(CancellationToken cancellationToken);

    Task<List<User>> GetUserById(int id, CancellationToken cancellationToken);

    Task<int> CreateUser(User user, CancellationToken cancellationToken);

    Task<int> UpdateUser(int id, int? roleId, string? login, string? passwordHash, CancellationToken cancellationToken);

    Task<int> DeleteUser(int id, CancellationToken cancellationToken);
}