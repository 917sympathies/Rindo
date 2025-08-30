using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;

namespace Application.Interfaces.Services;

public interface IAuthorizationService
{
    Task<User> SignUpUser(SignUpDto signUpDto);
    Task<Result<TokenDto>> AuthUser(LoginDto loginDto);
}