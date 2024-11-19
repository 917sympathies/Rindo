using Application.Mapping;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Repositories;
using Rindo.Domain.Services;
using Rindo.Infrastructure;
using Rindo.Infrastructure.Models;

namespace Application.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserRepository _userRepository;
    private readonly RindoDbContext _context;
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

    public async Task<Result<Tuple<UserDto, string>>> AuthUser(LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByUsername(loginDto.Username);
        if (user is null) return Error.NotFound("User with this username doesn't exists");
        
        if (!user.Password.Equals(loginDto.Password))
        {
            var pass = PasswordHandler.GetPasswordHash(loginDto.Password);
            if (!user.Password.Equals(pass))
                return Error.Validation("Wrong password");
        }

        var token = _jwtProvider.GenerateToken(user);
        var userDto = user.MapToDto();
        return Tuple.Create(userDto, token);
    }
}