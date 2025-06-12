using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;

namespace Application.Interfaces.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectShortInfoDto>> GetProjectsWhereUserAttends(Guid userId);
    Task<ProjectOnReturnDto?> GetProjectById(Guid id);
    Task<ProjectOnReturnDto> GetProjectSettings(Guid id);
    Task<ProjectHeaderInfoDto> GetProjectsInfoForHeader(Guid id);
    Task<Result<User>> InviteUserToProject(Guid projectId, string username, Guid senderId);
    Task<Result> AddUserToProject(Guid projectId, Guid userId);
    Task<Result> CreateProject(ProjectOnCreateDto projectOnCreateDto);
    Task<Result> RemoveUserFromProject(Guid projectId, string username);
    Task<Result> UpdateProjectName(Guid projectId, string name);
    Task<Result> UpdateProjectDescription(Guid projectId, string description);
    Task<Result> DeleteProject(Guid id);
    Task<Result> UpdateProjectStages(Guid projectId, Stage[] stages);
    Task<Result<object>> GetProjectsWithUserTasks(Guid userId);
}