using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Auth;
using Rindo.Domain.DataObjects;

namespace Application.Interfaces.Services;

public interface IAuthorizationService
{
    Task<User> SignUpUser(SignUpDto signUpDto);
    Task<TokenDto> AuthUser(LoginDto loginDto);
    Task<TokenDto> RefreshToken(string refreshToken, Guid userId);
}