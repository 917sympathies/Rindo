using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class CachedProjectRepository(ProjectRepository decorated, IDistributedCache distributedCache) : IProjectRepository
{
    public async Task CreateProject(Project project) => await decorated.CreateProject(project);

    public void DeleteProject(Project project)
    {
        var key = $"project-{project.Id}";
        distributedCache.Remove(key);
        decorated.DeleteProject(project);
    }

    public void UpdateProject(Project project)
    {
        var key = $"project-{project.Id}";
        distributedCache.Remove(key);
        decorated.UpdateProject(project);
    }

    public async Task<Project?> GetProjectById(Guid id)
    {
        var key = $"project-{id}";
        var cachedProject = await distributedCache.GetStringAsync(key);

        Project? project;
        
        if (string.IsNullOrEmpty(cachedProject))
        {
            project = await decorated.GetProjectById(id);

            if (project is null) return null;

            await distributedCache.SetStringAsync(key, 
                JsonConvert.SerializeObject(project, 
                    new JsonSerializerSettings 
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));

            return project;
        }

        project = JsonConvert.DeserializeObject<Project>(cachedProject);

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
        var key = $"project-{project.Id}";
        await distributedCache.RemoveAsync(key);
        await decorated.UpdateProperty(project, expression);
    }
}