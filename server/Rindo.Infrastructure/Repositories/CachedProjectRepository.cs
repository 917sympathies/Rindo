using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class CachedProjectRepository : IProjectRepository
{
    private readonly ProjectRepository _decorated;

    private readonly RindoDbContext _context;
    
    private readonly IDistributedCache _distributedCache;

    public CachedProjectRepository(ProjectRepository decorated, IDistributedCache distributedCache, RindoDbContext context)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
        _context = context;
    }

    public async Task CreateProject(Project project) =>
        await _decorated.CreateProject(project);

    public async Task DeleteProject(Project project)
    {
        var key = $"project-{project.Id}";
        await _distributedCache.RemoveAsync(key);
        await _decorated.DeleteProject(project);
    }

    public async Task UpdateProject(Project project)
    {
        var key = $"project-{project.Id}";
        await _distributedCache.RemoveAsync(key);
        await _decorated.UpdateProject(project);
    }

    public async Task<Project?> GetProjectById(Guid id)
    {
        var key = $"project-{id}";
        var cachedProject = await _distributedCache.GetStringAsync(key);

        Project? project;
        
        if (string.IsNullOrEmpty(cachedProject))
        {
            project = await _decorated.GetProjectById(id);

            if (project is null) return null;

            await _distributedCache.SetStringAsync(key, 
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

    public async Task<IEnumerable<Project>> GetProjectsByUserId(Guid userId)
    {
        return await _decorated.GetProjectsByUserId(userId);
    }

    public async Task<IEnumerable<Project>> GetProjectsWhereUserAttends(User user)
    {
        return await _decorated.GetProjectsWhereUserAttends(user);
    }

    public async Task UpdateProperty<TProperty>(Project project, Expression<Func<Project, TProperty>> expression)
    {
        var key = $"project-{project.Id}";
        await _distributedCache.RemoveAsync(key);
        await _decorated.UpdateProperty(project, expression);
    }
}