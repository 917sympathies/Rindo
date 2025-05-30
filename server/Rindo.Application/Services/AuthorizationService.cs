using System.ComponentModel.DataAnnotations;
using Application.Common.Mapping;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Repositories;
using Rindo.Domain.Services;
using Rindo.Infrastructure;
using Rindo.Infrastructure.Jwt;

namespace Application.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserRepository _userRepository;
    private readonly RindoDbContext _context; //TODO: remove DbContext
    private readonly IJwtProvider _jwtProvider;
    
    public AuthorizationService(IUserRepository userRepository, RindoDbContext context, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _context = context;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<Result> SignUpUser(SignUpDto signUpDto)
    {
        var isUserExist = await _userRepository.GetUserByUsername(signUpDto.Username) is not null;
        if (isUserExist) return Error.Validation("User with this name already exists");
        
        var user = signUpDto.MapToModel();
        user.Password = PasswordHandler.GetPasswordHash(signUpDto.Password);
        
        await _userRepository.CreateUser(user);
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result<TokenDto>> AuthUser(LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByUsername(loginDto.Username);
        if (user is null) return Error.NotFound("User with this username doesn't exists");
        
        var password = PasswordHandler.GetPasswordHash(loginDto.Password);
        if (!user.Password.Equals(password))
            throw new ValidationException("Wrong password");

        return new TokenDto
        {
            Token = _jwtProvider.GenerateToken(user),
            User = user.MapToDto()
        };
    }
}