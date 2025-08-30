using Application.Common.Exceptions;
using Application.Common.Mapping;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Rindo.Domain.DTO;
using Rindo.Domain.Enums;
using Rindo.Domain.Models;

namespace Application.Services;

public class ProjectService(
    IProjectRepository projectRepository,
    IUserRepository userRepository,
    // IDistributedCache distributedCache, // TODO: get rid of this
    IChatRepository chatRepository,
    IInvitationRepository invitationRepository,
    ITaskRepository taskRepository,
    ITagService tagService) : IProjectService
{
    public async Task<Project> CreateProject(ProjectOnCreateDto projectOnCreateDto)
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
        return await projectRepository.CreateProject(project);
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

    public async Task<User> InviteUserToProject(Guid projectId, string username, Guid senderId)
    {
        var user = await userRepository.GetUserByUsername(username);
        if(user is null) throw new NotFoundException<User>($"User with username = {username} was not found");
        var project = await projectRepository.GetProjectById(projectId);
        if(project is null) throw new NotFoundException(nameof(Project), projectId);
        var sender = await userRepository.GetUserById(senderId);
        if(sender is null) throw new NotFoundException(nameof(User), senderId);
                    
        var invitation = new Invitation { RecipientId = user.Id, ProjectId = projectId, SenderId = sender.Id };
        await invitationRepository.CreateInvitation(invitation);
        return user;
    }

    public async Task AddUserToProject(Guid projectId, Guid userId)
    {
        var invitation = (await invitationRepository.GetInvitationsByUserId(userId)).FirstOrDefault(x => x.ProjectId == projectId);
        if(invitation is null) throw new NotFoundException<Invitation>($"Invitation from project with id = {projectId} to user with id = ${userId} could not be found");
        var project = await projectRepository.GetProjectByIdWithUsers(projectId);
        if(project is null) throw new NotFoundException(nameof(Project), projectId);
        var user = await userRepository.GetUserById(userId);
        if(user is null) throw new NotFoundException(nameof(User), userId);
        
        await invitationRepository.DeleteInvitation(invitation);
        project.Users.Add(user);
        // TODO: add method to save users in project
    }

    public async Task RemoveUserFromProject(Guid projectId, string username)
    {
        var user = await userRepository.GetUserByUsername(username);
        if (user is null) throw new NotFoundException<User>($"User with username = {username} was not found");
        var project = await projectRepository.GetProjectById(projectId);
        if (project is null) throw new NotFoundException(nameof(Project), projectId);
        
        await taskRepository.UnassignTasksFromUser(projectId, user.Id);
        
        project.Users.Remove(user); //TODO: add method to remove users from project


        // await distributedCache.RemoveAsync($"project-{project.Id}");
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
        // await distributedCache.RemoveAsync($"project-{project.Id}");
    }

    public async Task<object> GetProjectsWithUserTasks(Guid userId)
    {
        var user = await userRepository.GetUserById(userId);
        if (user is null) throw new NotFoundException(nameof(User), userId);
        // TODO: easy refactor - just sort included in project tasks by userId
        var tasks = await taskRepository.GetTasksAssignedToUser(userId);
        var projects = await projectRepository.GetProjectsWhereUserAttends(userId);
        var result = projects.Select(p => new {p.Name, p.Id, tasks = tasks.Where(t => t.ProjectId == p.Id) }).ToList();
        return result;
    }
}