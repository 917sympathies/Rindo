using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public interface IProjectService
{
    Task<Result> CreateProject(ProjectOnCreateDto projectOnCreateDto);
    Task<ProjectOnReturnDto?> GetProjectById(Guid id);
    Task<ProjectOnReturnDto> GetProjectSettings(Guid id);
    Task<IEnumerable<ProjectInfoSidebar>> GetProjectsWhereUserAttends(Guid userId);
    Task<Result<User>> InviteUserToProject(Guid projectId, string username, string senderUsername);
    Task<Result<User>> AddUserToProject(Guid projectId, Guid userId);
    Task<Result> RemoveUserFromProject(Guid projectId, string username);
    Task<ProjectInfoHeader> GetProjectsInfoForHeader(Guid id);
    //Task<Result> UpdateProject(ProjectOnReturnDto project);
    Task<Result> UpdateProjectName(Guid projectId, string name);
    Task<Result> UpdateProjectDescription(Guid projectId, string description);
    Task<Result> DeleteProject(Guid id);
    Task<Result> UpdateProjectStages(Guid projectId, Stage[] stages);
}