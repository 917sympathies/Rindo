namespace Rindo.Domain.DTO.Auth;

public class TokenDto
{
    public UserDto User { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}