using System.Linq.Expressions;
using Application.Interfaces.Caching;
using Application.Interfaces.Repositories;
using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Projects;
using Rindo.Domain.DataObjects;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories.Cached;

public class CachedProjectRepository(ProjectRepository decorated, IRedisCacheService redisCacheService) : IProjectRepository
{
    public async Task<Project> CreateProject(Project project)
    {
        return await decorated.CreateProject(project);
    }

    public async Task DeleteProject(Project project)
    {
        await redisCacheService.RemoveAsync($"project-{project.ProjectId}");
        await decorated.DeleteProject(project);
    }

    public async Task UpdateProject(UpdateProjectDto updateProjectDto)
    {
        await redisCacheService.RemoveAsync($"project-{updateProjectDto.ProjectId}");
        await decorated.UpdateProject(updateProjectDto);
    }

    public async Task<ProjectHeaderInfoDto?> GetProjectHeaderInfo(Guid projectId)
    {
        await redisCacheService.RemoveAsync($"project-{projectId}");
        return await decorated.GetProjectHeaderInfo(projectId);
    }

    public async Task<Project?> GetProjectById(Guid projectId)
    {
        var cachedProject = await redisCacheService.GetAsync<Project>($"project-{projectId}");
        if (cachedProject is not null)
        {
            return cachedProject;
        }

        var project = await decorated.GetProjectById(projectId);

        if (project is null) return null;

        await redisCacheService.SetAsync($"project-{projectId}", project);

        return project;
    }

    public async Task AddUserToProject(Guid projectId, Guid userId)
    {
        await redisCacheService.RemoveAsync($"project-{projectId}");
        await decorated.AddUserToProject(projectId, userId);
    }
    
    public async Task RemoveUserFromProject(Guid projectId, Guid userId)
    {
        await redisCacheService.RemoveAsync($"project-{projectId}");
        await decorated.RemoveUserFromProject(projectId, userId);
    }

    public async Task<IEnumerable<Project>> GetProjectsOwnedByUser(Guid userId)
    {
        return await decorated.GetProjectsOwnedByUser(userId);
    }

    public async Task<IEnumerable<Project>> GetProjectsByIds(Guid[] projectsIds)
    {
        return await decorated.GetProjectsByIds(projectsIds);
    }

    public async Task<Project?> GetProjectByIdWithUsers(Guid projectId)
    {
        return await decorated.GetProjectByIdWithUsers(projectId);
    }

    public async Task<IEnumerable<Project>> GetProjectsWhereUserAttends(Guid userId)
    {
        return await decorated.GetProjectsWhereUserAttends(userId);
    }

    public async Task UpdateProperty<TProperty>(Project project, Expression<Func<Project, TProperty>> expression)
    {
        await redisCacheService.RemoveAsync($"project-{project.ProjectId}");
        await decorated.UpdateProperty(project, expression);
    }

    public async Task UpdateCollection<TProperty>(Project project, Expression<Func<Project, IEnumerable<TProperty>>> expression) where TProperty : class
    {
        await redisCacheService.RemoveAsync($"project-{project.ProjectId}");
        await decorated.UpdateCollection(project, expression);
    }
}