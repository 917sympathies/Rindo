using System.Runtime.InteropServices.JavaScript;
using Application.Interfaces.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly RindoDbContext _context;

    public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper, RindoDbContext context)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<Result> CreateProject(ProjectOnCreateDto projectOnCreateDto)
    {
        try
        {
            var project = _mapper.Map<Project>(projectOnCreateDto);
            var owner = await _userRepository.GetUserById(projectOnCreateDto.OwnerId);
            if (owner is null) return Result.Failure(Error.NotFound("Такого пользователя не существует!"));
            await _projectRepository.CreateProject(project);
            project.Owner = owner;
            project.Stages = new List<Stage>()
            {
                new Stage() { Name = "Запланированы", Index = 0},
                new Stage() { Name = "В процессе", Index = 1 },
                new Stage() { Name = "Завершены", Index = 2 },
            };
            project.Chat = new Chat(){ProjectId = project.Id};
            await _context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            return Result.Failure(Error.Failure("Не получилось создать проект!"));
        }
        return Result.Success();
    }

    public async Task<ProjectOnReturnDto?> GetProjectById(Guid id)
    {
        var project = await _projectRepository.GetProjectById(id);
        return _mapper.Map<ProjectOnReturnDto>(project);
    }

    public async Task<ProjectOnReturnDto> GetProjectSettings(Guid id)
    {
        var project = await _projectRepository.GetProjectById(id);
        return _mapper.Map<ProjectOnReturnDto>(project);
    }

    public async Task<IEnumerable<ProjectInfoSidebar>> GetProjectsWhereUserAttends(Guid userId)
    {
        var user = await _userRepository.GetUserById(userId);
        var projectsAttender = await _projectRepository.GetProjectsWhereUserAttends(user);
        var projectsOwner = await _projectRepository.GetProjectsByUserId(userId);
        var ret = new List<Project>(projectsAttender);
        ret.AddRange(projectsOwner);
        return _mapper.Map<ProjectInfoSidebar[]>(ret);
    }

    public async Task<Result<User>> InviteUserToProject(Guid projectId, string username, Guid senderId)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user is null) return Error.NotFound("Такого пользователя не существует");
        var project = await _projectRepository.GetProjectById(projectId);
        if (project is null) return Error.NotFound("Такого проекта не существует");
        var sender = await _userRepository.GetUserById(senderId);
        if (sender is null) return Error.NotFound("Такого пользователя не существует");
        var invitation = new Invitation() { UserId = user.Id, ProjectId = projectId, ProjectName = project.Name, SenderUsername = sender.Username};
        _context.Invitations.Add(invitation);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<Result> AddUserToProject(Guid id, Guid userId)
    {
        var invitation =
            await _context.Invitations.FirstOrDefaultAsync(inv =>
                inv.ProjectId == id && userId == inv.UserId);
        _context.Invitations.Remove(invitation);
        await _context.SaveChangesAsync();
        var project = await _context.Projects.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == id);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        project.Users.Add(user);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> RemoveUserFromProject(Guid id, string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null) return Error.NotFound("Такого пользователя не существует");
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        if (project is null) return Error.NotFound("Такого проекта не существует");
        
        await _context.Entry(project).Collection(p => p.Users).LoadAsync();
        project.Users.Remove(user);
            
        var upr = await _context.UserProjectRoles.Where(up => up.UserId == user.Id && up.ProjectId == id).ToListAsync();
        _context.UserProjectRoles.RemoveRange(upr);

        var tasks = await _context.Tasks.Where(t => t.ResponsibleUserId == user.Id && t.ProjectId == id).ToListAsync();
        foreach (var task in tasks) task.ResponsibleUserId = null;
            
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<ProjectInfoHeader> GetProjectsInfoForHeader(Guid id)
    {
        var project = await _projectRepository.GetProjectById(id);
        return _mapper.Map<ProjectInfoHeader>(project);
    }

    public async Task<Result> UpdateProjectName(Guid projectId, string name)
    {
        var project = await _projectRepository.GetProjectById(projectId);
        project!.Name = name;
        await _projectRepository.UpdateProperty(project, p => p.Name);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateProjectDescription(Guid projectId, string description)
    {
        var project = await _projectRepository.GetProjectById(projectId);
        project!.Description = description;
        await _projectRepository.UpdateProperty(project, p => p.Description);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteProject(Guid id)
    {
        var project = await _projectRepository.GetProjectById(id);
        if (project is null) return Result.Failure(Error.Failure("Такого проекта нет!"));
        var tags = await _context.Tags.Where(t => t.ProjectId == id).ToListAsync();
        _context.RemoveRange(tags);
        await _projectRepository.DeleteProject(project);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateProjectStages(Guid projectId, Stage[] stages)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        if (project is null) return Error.NotFound("Такого проекта не существует");
        project.Stages = stages;
        await _context.SaveChangesAsync(); 
        return Result.Success();
    }

    public async Task<Result<object>> GetProjectsWithUserTasks(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) return Error.NotFound("Такого пользователя не существует");
        var tasks = await _context.Tasks.Where(t => t.ResponsibleUserId == userId).ToListAsync();
        var projects = await _context.Projects.Where(p => p.Users.Contains(user) || p.OwnerId == user.Id).ToListAsync();
        var result = projects.Select(p => new {p.Name, p.Id, tasks = tasks.Where(t => t.ProjectId == p.Id) }).ToList();
        return result;
    }
}