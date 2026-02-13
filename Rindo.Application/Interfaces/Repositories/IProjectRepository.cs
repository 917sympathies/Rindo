using System.Linq.Expressions;
using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Projects;
using Rindo.Domain.DataObjects;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface  IProjectRepository
{
    Task<Project> CreateProject(Project project);
    Task DeleteProject(Project project);
    Task UpdateProject(UpdateProjectDto updateProjectDto);
    Task<ProjectHeaderInfoDto?> GetProjectHeaderInfo(Guid projectId);
    Task<Project?> GetProjectById(Guid projectId);
    Task<IEnumerable<Project>> GetProjectsByIds(Guid[] projectsIds);
    Task AddUserToProject(Guid projectId, Guid userId);
    Task RemoveUserFromProject(Guid projectId, Guid userId);
    Task<IEnumerable<Project>> GetProjectsOwnedByUser(Guid userId);
    Task<Project?> GetProjectByIdWithUsers(Guid projectId);
    Task<IEnumerable<Project>> GetProjectsWhereUserAttends(Guid userId);
    Task UpdateProperty<TProperty>(Project project, Expression<Func<Project, TProperty>> expression);
    Task UpdateCollection<TProperty>(Project project, Expression<Func<Project, IEnumerable<TProperty>>> expression) where TProperty : class;
}