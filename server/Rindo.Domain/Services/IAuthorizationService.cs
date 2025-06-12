using Rindo.Domain.Common;
using Rindo.Domain.DTO;

namespace Rindo.Domain.Services;

public interface IAuthorizationService
{
    Task<Result> SignUpUser(SignUpDto signUpDto);
    Task<Result<TokenDto>> AuthUser(LoginDto loginDto);
}