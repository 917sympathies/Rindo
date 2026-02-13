using System.IdentityModel.Tokens.Jwt;
using Application.Interfaces.Caching;

namespace Application.Auth;


public interface IAuthCacheService
{
    Task InsertRefreshTokenAsync(string refreshToken, JwtSecurityToken refreshTokenValue);
    Task<JwtSecurityToken?> GetRefreshTokenAsync(string refreshToken);
}

public class AuthCacheService(IRedisCacheService redisCacheService): IAuthCacheService
{
    private const string Prefix = "auth";

    public async Task InsertRefreshTokenAsync(string refreshToken, JwtSecurityToken refreshTokenValue)
    {
        await redisCacheService.SetAsync($"{Prefix}-refresh-${refreshToken}", refreshTokenValue);
    }

    public async Task<JwtSecurityToken?> GetRefreshTokenAsync(string refreshToken)
    {
        return await redisCacheService.GetAsync<JwtSecurityToken>($"{Prefix}-refresh-${refreshToken}");
    }
}