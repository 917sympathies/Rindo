using Application.Common.Exceptions;
using Application.Common.Mapping;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Enums;
using Rindo.Domain.Models;

namespace Application.Services;

public class ProjectService(
    IProjectRepository projectRepository,
    IUserRepository userRepository,
    IDistributedCache distributedCache, // TODO: get rid of this
    IChatRepository chatRepository,
    IInvitationService invitationService,
    ITaskRepository taskRepository,
    ITagService tagService) : IProjectService
{
    public async Task<Result> CreateProject(ProjectOnCreateDto projectOnCreateDto)
    {
        var chat = await chatRepository.Create(new Chat());
        var project = new Project
        {
            Name = projectOnCreateDto.Name,
            Description = projectOnCreateDto.Description,
            OwnerId = projectOnCreateDto.OwnerId,
            CreatedDate = projectOnCreateDto.StartDate,
            Tags = projectOnCreateDto.Tags,
            Stages = [
                new Stage { Name = "Запланированы", Index = 0, Type = StageType.Base },
                new Stage { Name = "В процессе", Index = 1, Type = StageType.Base },
                new Stage { Name = "Завершены", Index = 2, Type = StageType.Base }
            ],
            ChatId = chat.Id
        };
        await projectRepository.CreateProject(project);
        return Result.Success();
    }

    public async Task<ProjectOnReturnDto?> GetProjectById(Guid projectId)
    {
        var project = await projectRepository.GetProjectById(projectId);
        if (project is null)
        {
            throw new NotFoundException(nameof(Project), projectId);
        }
        return project.MapToDto();
    }

    public async Task<ProjectOnReturnDto> GetProjectSettings(Guid projectId)
    {
        var project = await projectRepository.GetProjectById(projectId);
        if (project == null)
        {
            throw new NotFoundException(nameof(Project), projectId);
        }
        // weird logic
        var projectOwner = await userRepository.GetUserById(project.OwnerId);
        project.Users.Add(projectOwner!);
        return project.MapToDto();
    }

    public async Task<IEnumerable<ProjectShortInfoDto>> GetProjectsWhereUserAttends(Guid userId)
    {
        var projectsAttender = await projectRepository.GetProjectsWhereUserAttends(userId);
        var projectsOwner = await projectRepository.GetProjectsOwnedByUser(userId);
        var ret = new List<Project>(projectsAttender);
        ret.AddRange(projectsOwner);
        return ret.Select(x => x.MapToSidebarDto());
    }

    public async Task<Result<User>> InviteUserToProject(Guid projectId, string username, Guid senderId)
    {
        var user = await userRepository.GetUserByUsername(username);
        var project = await projectRepository.GetProjectById(projectId);
        var sender = await userRepository.GetUserById(senderId);
        
        if (user is null || project is null || sender is null)
            return Error.NotFound("Ошибка при приглашении пользователя в проект");
                    
        var invitation = new Invitation { RecipientId = user.Id, ProjectId = projectId, SenderId = sender.Id };
        await invitationService.CreateInvitation(invitation);
        return user;
    }

    public async Task<Result> AddUserToProject(Guid projectId, Guid userId)
    {
        var invitation = (await invitationService.GetInvitationsByUserId(userId)).FirstOrDefault(x => x.ProjectId == projectId);
        // var invitation = await _context.Invitations.FirstOrDefaultAsync(inv => inv.ProjectId == projectId && userId == inv.RecipientId);
        var project = await projectRepository.GetProjectByIdWithUsers(projectId);
        // var project = await _context.Projects.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == projectId);
        // var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var user = await userRepository.GetUserById(userId);
        
        if (user is null || project is null || invitation is null)
            return Error.NotFound(""); // adding user to project error
        
        await invitationService.DeleteInvitation(invitation.Id);
        // _context.Invitations.Remove(invitation);
        project.Users.Add(user);
//        // await _context.SaveChangesAsync();
        await distributedCache.RemoveAsync($"project-{project.Id}");
        return Result.Success();
    }

    public async Task<Result> RemoveUserFromProject(Guid projectId, string username)
    {
        var user = await userRepository.GetUserByUsername(username);
        if (user is null) return Error.NotFound("User with this id doesn't exists");
        var project = await projectRepository.GetProjectById(projectId);
        if (project is null) return Error.NotFound("Project with this id doesn't exists");
        
        // await _context.Entry(project).Collection(p => p.Users).LoadAsync();
        project.Users.Remove(user);

        var tasks = await taskRepository.GetTasksByUserId(user.Id);
        // var tasks = await _context.Tasks.Where(t => t.AsigneeId == user.Id && t.ProjectId == projectId).ToListAsync();
        foreach (var task in tasks) task.AsigneeId = null;
            
        //await _context.SaveChangesAsync();
        await distributedCache.RemoveAsync($"project-{project.Id}");
        return Result.Success();
    }

    public async Task<ProjectHeaderInfoDto> GetProjectsInfoForHeader(Guid projectId)
    {
        var project = await projectRepository.GetProjectById(projectId);
        if (project is null)
        {
            throw new NotFoundException(nameof(Project), projectId);
        }
        return project.MapToHeaderDto();
    }

    public async Task UpdateProjectName(Guid projectId, string name)
    {
        var project = await projectRepository.GetProjectById(projectId);
        project!.Name = name;
        await projectRepository.UpdateProperty(project, p => p.Name);
        //await _context.SaveChangesAsync();
    }

    public async Task UpdateProjectDescription(Guid projectId, string description)
    {
        var project = await projectRepository.GetProjectById(projectId);
        project!.Description = description;
        await projectRepository.UpdateProperty(project, p => p.Description);
        //await _context.SaveChangesAsync();
    }

    public async Task DeleteProject(Guid projectId)
    {
        var project = await projectRepository.GetProjectById(projectId);
        if (project is null) throw new NotFoundException(nameof(Project), projectId);
        var tags = await tagService.GetTagsByProjectId(projectId);
        // var tags = await _context.Tags.Where(t => t.ProjectId == projectId).ToListAsync();
        await tagService.DeleteTagsByProjectId(projectId);
        projectRepository.DeleteProject(project);
    }

    public async Task UpdateProjectStages(Guid projectId, Stage[] stages)
    {
        var project = await projectRepository.GetProjectById(projectId);
        if (project is null) throw new NotFoundException(nameof(Project), projectId);
        project.Stages = stages;
        //await _context.SaveChangesAsync();
        await distributedCache.RemoveAsync($"project-{project.Id}");
    }

    public async Task<Result<object>> GetProjectsWithUserTasks(Guid userId)
    {
        var user = await userRepository.GetUserById(userId);
        if (user is null) throw new NotFoundException(nameof(User), userId);
        var tasks = await taskRepository.GetTasksByUserId(userId);
        // var tasks = await _context.Tasks.Where(t => t.AsigneeId == userId).ToListAsync();
        // var projects = await _context.Projects.Where(p => p.Users.Contains(user) || p.OwnerId == user.Id).ToListAsync();
        var projects = await projectRepository.GetProjectsWhereUserAttends(userId);
        var result = projects.Select(p => new {p.Name, p.Id, tasks = tasks.Where(t => t.ProjectId == p.Id) }).ToList();
        return result;
    }
}