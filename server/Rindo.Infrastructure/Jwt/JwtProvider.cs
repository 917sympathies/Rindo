﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rindo.Domain.Models;

namespace Rindo.Infrastructure.Jwt;

public interface IJwtProvider
{
    string GenerateToken(User user);
}

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;
    public JwtProvider(IOptions<JwtOptions> options) => _options = options.Value;
    public string GenerateToken(User user)
    {
        Claim[] claims =
        [
            new ("userId", user.Id.ToString())
        ];
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials, expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }
}