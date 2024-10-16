using AutoMapper;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Domain.Services;
using Rindo.Infrastructure;
using Rindo.Infrastructure.Models;

namespace Application.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly RindoDbContext _context;
    private readonly IJwtProvider _jwtProvider;
    
    public AuthorizationService(IUserRepository userRepository, IMapper mapper, RindoDbContext context, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _context = context;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<Result> SignUpUser(UserDtoSignUp userDtoSignUp)
    {
        var isUserExist = await _userRepository.GetUserByUsername(userDtoSignUp.Username) is not null;
        if (isUserExist) return Error.Validation("Пользователь с таким именем уже существует");
        
        var user = _mapper.Map<User>(userDtoSignUp);
        user.Password = PasswordHandler.GetPasswordHash(userDtoSignUp.Password);
        
        await _userRepository.CreateUser(user);
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result<Tuple<UserDto, string>>> AuthUser(UserDtoAuth userDtoAuth)
    {
        var user = await _userRepository.GetUserByUsername(userDtoAuth.Username);
        if (user is null) return Error.NotFound("Пользователя с таким именем пользователя не существует");
        
        if (!user.Password.Equals(userDtoAuth.Password))
        {
            var pass = PasswordHandler.GetPasswordHash(userDtoAuth.Password);
            if (!user.Password.Equals(pass))
                return Error.Validation("Неверный пароль");
        }

        var token = _jwtProvider.GenerateToken(user);
        var userDto = _mapper.Map<UserDto>(user);
        return Tuple.Create(userDto, token);
    }
}