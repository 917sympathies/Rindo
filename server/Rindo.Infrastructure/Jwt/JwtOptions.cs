namespace Rindo.Infrastructure;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresHours { get; set; }
    public string CookiesName { get; set; }
}