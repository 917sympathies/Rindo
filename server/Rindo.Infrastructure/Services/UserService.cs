using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using Rindo.Domain;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure;
using Task = System.Threading.Tasks.Task;

namespace Application.Services.UserService;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    private readonly IJwtProvider _jwtProvider;
    
    public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper, IJwtProvider jwtProvider, IProjectRepository projectRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<User?>> GetUserById(Guid id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user is null) return Error.NotFound("Пользователя с таким именем не существует!");
        return user;
    }

    public async Task<UserDto?> GetUserInfo(Guid id)
    {
        var user = await _userRepository.GetUserById(id);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<Result<IEnumerable<UserDto>>> GetUsersByProjectId(Guid projectId)
    {
        var project = await _projectRepository.GetProjectById(projectId);
        if (project is null) return Error.NotFound("Такого проекта не существует!");
        // var users = project.Users;
        // users.Add(project.Owner);
        var users = project.Users.ToList();
        users.Add(project.Owner);
        return _mapper.Map<UserDto[]>(users);
    }

    public async Task<Result<Tuple<User, string>>> SignUpUser(UserDtoSignUp userDtoSignUp)
    {
        var isUserExist = (await _userRepository.GetUserByUsername(userDtoSignUp.Username)) is not null;
        if (isUserExist) return Error.Validation("Пользователь с таким именем уже существует");
        var user = _mapper.Map<User>(userDtoSignUp);
        user.Password = PasswordHandler.GetPasswordHash(userDtoSignUp.Password);
        await _userRepository.CreateUser(user);
        await _unitOfWork.SaveAsync();
        var token = _jwtProvider.GenerateToken(user);
        return Tuple.Create(user, token);
    }

    public async Task<Result<Tuple<User, string>>> AuthUser(UserDtoAuth userDtoAuth)
    {
        var user = await _userRepository.GetUserByUsername(userDtoAuth.Username);
        if (user is null) return Error.NotFound("Пользователя с таким именем пользователя не существует!");
        if (!user.Password.Equals(userDtoAuth.Password))
        {
            var pass = PasswordHandler.GetPasswordHash(userDtoAuth.Password);
            if (!user.Password.Equals(pass))
                return Error.Validation("Неверный пароль!");
        }

        var token = _jwtProvider.GenerateToken(user);
        return Tuple.Create(user, token);
    }
}