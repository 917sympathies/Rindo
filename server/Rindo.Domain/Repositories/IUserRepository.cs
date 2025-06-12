using Rindo.Domain.DTO;
using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;

public interface IUserRepository
{
    Task CreateUser(User user);
    void DeleteUser(User user);
    void UpdateUser(User user);
    Task<User?> GetUserById(Guid id);
    Task<User?> GetUserByUsername(string username);
    //Task<IEnumerable<UserDto>> GetUsersByProjectId(Guid projectId);
}