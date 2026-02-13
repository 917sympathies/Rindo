using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Projects;
using Rindo.Domain.DTO.Tasks;
using Rindo.Domain.DataObjects;

namespace Application.Interfaces.Services;

public interface ITaskService
{
    Task<ProjectTask> CreateTask(AddTaskDto addTaskDto);
    Task UpdateTask(UpdateTaskDto projectTaskDto);
    Task UpdateTaskStage(Guid taskId, Guid stageId);
    Task DeleteTask(Guid taskId);
    Task DeleteTasksByProjectId(Guid projectId);
    Task UnassignTasksFromUser(Guid projectId, Guid userId);
    Task<IEnumerable<ProjectTaskDto>> GetTasksByProjectId(Guid projectId);
    Task<IEnumerable<ProjectTask>> GetTasksByUserId(Guid userId);
    Task<ProjectTaskDto> GetTaskById(Guid id);
}