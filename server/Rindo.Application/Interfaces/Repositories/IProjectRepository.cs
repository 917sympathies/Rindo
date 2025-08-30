using System.Linq.Expressions;
using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface  IProjectRepository
{
    Task<Project> CreateProject(Project project);
    
    void DeleteProject(Project project);
    
    void UpdateProject(Project project);
    
    Task<Project?> GetProjectById(Guid id);
    
    Task<IEnumerable<Project>> GetProjectsOwnedByUser(Guid userId);

    Task<Project?> GetProjectByIdWithUsers(Guid projectId);
    
    Task<IEnumerable<Project>> GetProjectsWhereUserAttends(Guid userId);
    
    Task UpdateProperty<TProperty>(Project project,
        Expression<Func<Project, TProperty>> expression);
}