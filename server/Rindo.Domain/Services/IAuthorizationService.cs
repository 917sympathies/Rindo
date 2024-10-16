using Rindo.Domain.Common;
using Rindo.Domain.DTO;

namespace Rindo.Domain.Services;

public interface IAuthorizationService
{
    Task<Result> SignUpUser(UserDtoSignUp userDtoSignUp);
    Task<Result<Tuple<UserDto, string>>> AuthUser(UserDtoAuth userDtoAuth);
}