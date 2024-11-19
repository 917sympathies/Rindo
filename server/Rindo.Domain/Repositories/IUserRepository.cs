using Rindo.Domain.DTO;
using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;

public interface IUserRepository
{
    Task CreateUser(User user);
    Task DeleteUser(User user);
    Task UpdateUser(User user);
    Task<User?> GetUserById(Guid id);
    Task<User?> GetUserByUsername(string username);
    //Task<IEnumerable<UserDto>> GetUsersByProjectId(Guid projectId);
}