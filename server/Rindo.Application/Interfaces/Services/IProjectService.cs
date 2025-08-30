using Rindo.Domain.DTO;
using Rindo.Domain.Models;

namespace Application.Interfaces.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectShortInfoDto>> GetProjectsWhereUserAttends(Guid userId);
    Task<ProjectOnReturnDto?> GetProjectById(Guid projectId);
    Task<ProjectOnReturnDto> GetProjectSettings(Guid projectId);
    Task<ProjectHeaderInfoDto> GetProjectsInfoForHeader(Guid projectId);
    Task<User> InviteUserToProject(Guid projectId, string username, Guid senderId);
    Task AddUserToProject(Guid projectId, Guid userId);
    Task<Project> CreateProject(ProjectOnCreateDto projectOnCreateDto);
    Task RemoveUserFromProject(Guid projectId, string username);
    Task UpdateProjectName(Guid projectId, string name);
    Task UpdateProjectDescription(Guid projectId, string description);
    Task DeleteProject(Guid projectId);
    Task UpdateProjectStages(Guid projectId, Stage[] stages);
    Task<object> GetProjectsWithUserTasks(Guid userId);
}