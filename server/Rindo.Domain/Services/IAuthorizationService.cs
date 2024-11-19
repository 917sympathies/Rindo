using Rindo.Domain.Common;
using Rindo.Domain.DTO;

namespace Rindo.Domain.Services;

public interface IAuthorizationService
{
    Task<Result> SignUpUser(SignUpDto signUpDto);
    Task<Result<Tuple<UserDto, string>>> AuthUser(LoginDto loginDto);
}