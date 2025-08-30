using System.ComponentModel.DataAnnotations;
using Application.Common.Mapping;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;
using Rindo.Infrastructure.Jwt;

namespace Application.Services;

public class AuthorizationService(IUserRepository userRepository, IJwtProvider jwtProvider) : IAuthorizationService
{
    public async Task<User> SignUpUser(SignUpDto signUpDto)
    {
        var isUserExist = await userRepository.GetUserByUsername(signUpDto.Username) is not null;
        if (isUserExist) throw new ValidationException("User with this name already exists");
        
        var user = signUpDto.MapToModel();
        user.Password = PasswordHandler.GetPasswordHash(signUpDto.Password);
        
        return await userRepository.CreateUser(user);
    }

    public async Task<Result<TokenDto>> AuthUser(LoginDto loginDto)
    {
        var user = await userRepository.GetUserByUsername(loginDto.Username);
        if (user is null) return Error.NotFound("User with this username doesn't exists");
        
        var password = PasswordHandler.GetPasswordHash(loginDto.Password);
        if (!user.Password.Equals(password))
            throw new ValidationException("Wrong password");

        return new TokenDto
        {
            Token = jwtProvider.GenerateToken(user),
            User = user.MapToDto()
        };
    }
}