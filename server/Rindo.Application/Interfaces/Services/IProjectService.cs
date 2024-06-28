using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectInfoSidebar>> GetProjectsWhereUserAttends(Guid userId);
    Task<ProjectOnReturnDto?> GetProjectById(Guid id);
    Task<ProjectOnReturnDto> GetProjectSettings(Guid id);
    Task<ProjectInfoHeader> GetProjectsInfoForHeader(Guid id);
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