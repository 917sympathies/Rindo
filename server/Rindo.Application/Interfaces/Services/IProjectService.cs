using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;

namespace Application.Interfaces.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectShortInfoDto>> GetProjectsWhereUserAttends(Guid userId);
    Task<ProjectOnReturnDto?> GetProjectById(Guid projectId);
    Task<ProjectOnReturnDto> GetProjectSettings(Guid projectId);
    Task<ProjectHeaderInfoDto> GetProjectsInfoForHeader(Guid projectId);
    Task<Result<User>> InviteUserToProject(Guid projectId, string username, Guid senderId);
    Task<Result> AddUserToProject(Guid projectId, Guid userId);
    Task<Result> CreateProject(ProjectOnCreateDto projectOnCreateDto);
    Task<Result> RemoveUserFromProject(Guid projectId, string username);
    Task UpdateProjectName(Guid projectId, string name);
    Task UpdateProjectDescription(Guid projectId, string description);
    Task DeleteProject(Guid projectId);
    Task UpdateProjectStages(Guid projectId, Stage[] stages);
    Task<Result<object>> GetProjectsWithUserTasks(Guid userId);
}