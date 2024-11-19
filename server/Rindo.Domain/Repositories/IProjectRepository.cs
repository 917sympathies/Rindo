using System.Linq.Expressions;
using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;

public interface  IProjectRepository
{
    Task CreateProject(Project project);
    
    Task DeleteProject(Project project);
    
    Task UpdateProject(Project project);
    
    Task<Project?> GetProjectById(Guid id);
    
    Task<IEnumerable<Project>> GetProjectsByUserId(Guid userId);
    
    Task<IEnumerable<Project>> GetProjectsWhereUserAttends(User user);
    
    Task UpdateProperty<TProperty>(Project project,
        Expression<Func<Project, TProperty>> expression);
}