namespace Application.Auth.Jwt;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresMinutes { get; set; }
    public int RefreshTokenExpiresDays { get; set; }
    public string CookiesName { get; set; }
}