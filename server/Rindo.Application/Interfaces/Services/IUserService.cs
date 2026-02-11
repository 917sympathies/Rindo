using Rindo.Domain.DTO;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<UserDto> GetUserById(Guid id);
    Task UpdateUser(UserDto userDto);
    Task<IEnumerable<UserDto>> GetUsersByProjectId(Guid projectId);
}