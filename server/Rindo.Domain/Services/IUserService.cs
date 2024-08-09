using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<Result> SignUpUser(UserDtoSignUp userDtoSignUp);
    Task<Result<Tuple<UserDto, string>>> AuthUser(UserDtoAuth userDtoAuth);
    Task<Result<User?>> GetUserById(Guid id);
    Task<UserDto?> GetUserInfo(Guid id);
    Task<Result> ChangeUserLastName(Guid id, string lastName);
    Task<Result> ChangeUserFirstName(Guid id, string firstName);
    Task<Result<IEnumerable<UserDto>>> GetUsersByProjectId(Guid projectId);
}