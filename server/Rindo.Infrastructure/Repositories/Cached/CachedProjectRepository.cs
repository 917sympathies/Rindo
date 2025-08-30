using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Rindo.Domain.Models;
using Rindo.Infrastructure.Interfaces.Caching;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories.Cached;

public class CachedProjectRepository(ProjectRepository decorated, IRedisCacheService redisCacheService) : IProjectRepository
{
    public async Task<Project> CreateProject(Project project)
    {
        return await decorated.CreateProject(project);
    }

    public void DeleteProject(Project project)
    {
        redisCacheService.Remove($"project-{project.Id}");
        decorated.DeleteProject(project);
    }

    public void UpdateProject(Project project)
    {
        redisCacheService.Remove($"project-{project.Id}");
        decorated.UpdateProject(project);
    }

    public async Task<Project?> GetProjectById(Guid id)
    {
        var cachedProject = await redisCacheService.GetAsync<Project>($"project-{id}");
        if (cachedProject is not null)
        {
            return cachedProject;
        }

        var project = await decorated.GetProjectById(id);

        if (project is null) return null;

        await redisCacheService.SetAsync($"project-{id}", project);

        return project;
    }

    public async Task<IEnumerable<Project>> GetProjectsOwnedByUser(Guid userId)
    {
        return await decorated.GetProjectsOwnedByUser(userId);
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
        await redisCacheService.RemoveAsync($"project-{project.Id}");
        await decorated.UpdateProperty(project, expression);
    }
}