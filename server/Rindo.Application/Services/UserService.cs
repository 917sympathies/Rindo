using Application.Interfaces.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    private readonly IProjectRepository _projectRepository;
    
    private readonly IMapper _mapper;
    
    private readonly RindoDbContext _context;
    
    public UserService(IUserRepository userRepository, IMapper mapper, IProjectRepository projectRepository, RindoDbContext context)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _context = context;
        _mapper = mapper;
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

    public async Task<Result> ChangeUserLastName(Guid id, string lastName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) return Error.NotFound("Пользователя с таким идентификатором не существует");
        
        user.LastName = lastName;
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> ChangeUserFirstName(Guid id, string firstName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) return Error.NotFound("Пользователя с таким идентификатором не существует");
        
        user.FirstName = firstName;
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<IEnumerable<UserDto>>> GetUsersByProjectId(Guid projectId)
    {
        var project = await _projectRepository.GetProjectById(projectId);
        if (project is null) return Error.NotFound("Такого проекта не существует");
        
        var users = project.Users.ToList();
        var owner = await _context.Users.FirstOrDefaultAsync(u => u.Id == project.OwnerId);
        users.Add(owner);
        //users.Add(project.Owner);
        return _mapper.Map<UserDto[]>(users);
    }
}