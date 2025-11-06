using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions
{
    public interface IUsersRepository
    {
        Task<int> CreateUser(User user);
        Task<int> DeleteUser(int id);
        Task<List<User>> GetUser();
        Task<int> UpdateUser(int id, int roleId, string login, string passwordHash);
    }
}