﻿using Application.Common.Mapping;
using Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    
    private readonly IUserRepository _userRepository;
    
    private readonly PostgresDbContext _context; //TODO: remove DbContext

    private readonly IDistributedCache _distributedCache;

    public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository, PostgresDbContext context, IDistributedCache distributedCache)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _context = context;
        _distributedCache = distributedCache;
    }

    public async Task<Result> CreateProject(ProjectOnCreateDto projectOnCreateDto)
    {
        var chat = (await _context.Chats.AddAsync(new Chat())).Entity;
        var project = new Project
        {
            Name = projectOnCreateDto.Name,
            Description = projectOnCreateDto.Description,
            OwnerId = projectOnCreateDto.OwnerId,
            CreatedDate = projectOnCreateDto.StartDate,
            FinishDate = projectOnCreateDto.FinishDate,
            Tags = projectOnCreateDto.Tags,
            Stages = new List<Stage>
            {
                new() { Name = "Запланированы", Index = 0},
                new() { Name = "В процессе", Index = 1 },
                new() { Name = "Завершены", Index = 2 }
            },
            ChatId = chat.Id
        };
        await _projectRepository.CreateProject(project);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<ProjectOnReturnDto?> GetProjectById(Guid id)
    {
        var project = await _projectRepository.GetProjectById(id);
        return project?.MapToDto();
    }

    public async Task<ProjectOnReturnDto> GetProjectSettings(Guid id)
    {
        var project = await _projectRepository.GetProjectById(id);
        var projectOwner = await _context.Users.FirstOrDefaultAsync(u => u.Id == project!.OwnerId);
        project!.Users.Add(projectOwner!);
        return project.MapToDto();
    }

    public async Task<IEnumerable<ProjectShortInfoDto>> GetProjectsWhereUserAttends(Guid userId)
    {
        var user = await _userRepository.GetUserById(userId);
        var projectsAttender = await _projectRepository.GetProjectsWhereUserAttends(user);
        var projectsOwner = await _projectRepository.GetProjectsByUserId(userId);
        var ret = new List<Project>(projectsAttender);
        ret.AddRange(projectsOwner);
        return ret.Select(x => x.MapToSidebarDto());
    }

    public async Task<Result<User>> InviteUserToProject(Guid projectId, string username, Guid senderId)
    {
        var user = await _userRepository.GetUserByUsername(username);
        var project = await _projectRepository.GetProjectById(projectId);
        var sender = await _userRepository.GetUserById(senderId);
        
        if (user is null || project is null || sender is null)
            return Error.NotFound("Ошибка при приглашении пользователя в проект");
                    
        var invitation = new Invitation { RecipientId = user.Id, ProjectId = projectId, SenderId = sender.Id };
        _context.Invitations.Add(invitation);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<Result> AddUserToProject(Guid id, Guid userId)
    {
        var invitation =
            await _context.Invitations.FirstOrDefaultAsync(inv =>
                inv.ProjectId == id && userId == inv.RecipientId);
        var project = await _context.Projects.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == id);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if (user is null || project is null || invitation is null)
            return Error.NotFound(""); // adding user to project error
        
        _context.Invitations.Remove(invitation);
        project.Users.Add(user);
        await _context.SaveChangesAsync();
        await _distributedCache.RemoveAsync($"project-{project.Id}");
        return Result.Success();
    }

    public async Task<Result> RemoveUserFromProject(Guid id, string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null) return Error.NotFound("User with this id doesn't exists");
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        if (project is null) return Error.NotFound("Project with this id doesn't exists");
        
        await _context.Entry(project).Collection(p => p.Users).LoadAsync();
        project.Users.Remove(user);

        var tasks = await _context.Tasks.Where(t => t.AsigneeUserId == user.Id && t.ProjectId == id).ToListAsync();
        foreach (var task in tasks) task.AsigneeUserId = null;
            
        await _context.SaveChangesAsync();
        await _distributedCache.RemoveAsync($"project-{project.Id}");
        return Result.Success();
    }

    public async Task<ProjectHeaderInfoDto> GetProjectsInfoForHeader(Guid id)
    {
        var project = await _projectRepository.GetProjectById(id);
        return project.MapToHeaderDto();
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
        if (project is null) return Result.Failure(Error.Failure("Project with this id doesn't exists"));
        var tags = await _context.Tags.Where(t => t.ProjectId == id).ToListAsync();
        _context.RemoveRange(tags);
        _projectRepository.DeleteProject(project);
        return Result.Success();
    }

    public async Task<Result> UpdateProjectStages(Guid projectId, Stage[] stages)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        if (project is null) return Error.NotFound("Project with this id doesn't exists");
        project.Stages = stages;
        await _context.SaveChangesAsync();
        await _distributedCache.RemoveAsync($"project-{project.Id}");
        return Result.Success();
    }

    public async Task<Result<object>> GetProjectsWithUserTasks(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) return Error.NotFound("User with this id doesn't exists");
        var tasks = await _context.Tasks.Where(t => t.AsigneeUserId == userId).ToListAsync();
        var projects = await _context.Projects.Where(p => p.Users.Contains(user) || p.OwnerId == user.Id).ToListAsync();
        var result = projects.Select(p => new {p.Name, p.Id, tasks = tasks.Where(t => t.ProjectId == p.Id) }).ToList();
        return result;
    }
}