using Rindo.Domain.DataObjects;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> CreateUser(User user);
    Task DeleteUser(User user);
    Task UpdateUser(User user);
    Task<User?> GetUserById(Guid id);
    Task<User?> GetUserByUsername(string username);
    //Task<IEnumerable<UserDto>> GetUsersByProjectId(Guid projectId);
    Task<User[]> GetUsersByIds(Guid[] ids);
}