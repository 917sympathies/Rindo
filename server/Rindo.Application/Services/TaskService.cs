using Application.Common.Exceptions;
using Application.Common.Mapping;
using Application.Interfaces.Access;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Projects;
using Rindo.Domain.DTO.Tasks;
using Rindo.Domain.DataObjects;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class TaskService(
    ITaskRepository taskRepository,
    ITaskCommentRepository taskCommentRepository,
    IUserRepository userRepository,
    IProjectService projectService,
    IDataAccessController dataAccessController) : ITaskService
{
    public async Task<ProjectTask> CreateTask(AddTaskDto addTaskDto)
    {
        var project = await projectService.GetProjectById(addTaskDto.ProjectId);
        if (project is null)
        {
            throw new NotFoundException($"Project with id = {addTaskDto.ProjectId} not found");
        }
        var projectTasks = await taskRepository.GetTasksByProjectId(addTaskDto.ProjectId);
        var taskNumber = await taskRepository.GetNextTaskNumber(addTaskDto.ProjectId);
        var createdTask = new ProjectTask
        {
            TaskId = addTaskDto.Id,
            ProjectId = addTaskDto.ProjectId,
            TaskNumber = GetTaskSignature(project.Name, taskNumber),
            Name = addTaskDto.Name,
            Description = addTaskDto.Description,
            Priority = addTaskDto.Priority,
            StageId = addTaskDto.StageId,
            Index = projectTasks.Count(x => x.StageId == addTaskDto.StageId),
            AssigneeId = addTaskDto.AssigneeId,
            ReporterId = addTaskDto.ReporterId,
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow,
            DeadlineDate = addTaskDto.DeadlineDate,
            Comments = []
        };
        return await taskRepository.CreateTask(createdTask);
    }

    private string GetTaskSignature(string projectName, int taskNumber)
    {
        var projectShortName = string.Concat(projectName.Split()
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Select(word => word[0]));
        return $"{projectShortName}-{taskNumber}";
    }

    public async Task UpdateTask(UpdateTaskDto projectTaskDto)
    {
        await taskRepository.UpdateTask(projectTaskDto, dataAccessController.EmployeeId, DateTime.UtcNow);
    }

    public async Task DeleteTask(Guid taskId)
    {
        var task = await taskRepository.GetById(taskId);
        if (task is null) throw new NotFoundException(nameof(Task), taskId);
        await taskRepository.DeleteTask(task);
    }

    public async Task<IEnumerable<ProjectTaskDto>> GetTasksByProjectId(Guid projectId)
    {
        var tasks = (await taskRepository.GetTasksByProjectId(projectId)).ToArray();
        var assigneeUsersIds = tasks.Where(x => x.AssigneeId.HasValue).Select(x => x.AssigneeId!.Value).ToArray();
        var users = await userRepository.GetUsersByIds(assigneeUsersIds);

        var tasksDtos = tasks.Select(s => s.MapToDto()).ToArray();

        foreach (var taskDto in tasksDtos.Where(x => x.Assignee is not null))
        {
            var assignee = users.First(x => x.UserId == taskDto.Assignee!.Id);
            taskDto.Assignee.FirstName = assignee.FirstName;
            taskDto.Assignee.LastName = assignee.LastName;
        }
        return tasksDtos;
    }

    public async Task UpdateTaskStage(Guid taskId, Guid stageId)
    {
        var task = await taskRepository.GetById(taskId);
        if (task is null) throw new NotFoundException(nameof(Task), taskId);
        task.StageId = stageId;
        await taskRepository.UpdateProperty(task, projectTask => projectTask.StageId);
    }

    public async Task UnassignTasksFromUser(Guid projectId, Guid userId)
    {
        await taskRepository.UnassignTasksFromUser(projectId, userId);
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksByUserId(Guid userId)
    {
        var tasks = await taskRepository.GetTasksAssignedToUser(userId);
        return tasks;
    }

    public async Task DeleteTasksByProjectId(Guid projectId)
    {
        await taskRepository.DeleteTasksByProjectId(projectId);
    }

    public async Task<ProjectTaskDto> GetTaskById(Guid id)
    {
        var task = await taskRepository.GetById(id);
        if (task is null) throw new NotFoundException($"Task with this id doesn't exists {id}");
        var taskDto = task.MapToDto();
        if (task.AssigneeId.HasValue)
        {
            var assignee = await userRepository.GetUserById(task.AssigneeId.Value);
            taskDto.Assignee = new UserShortInfoDto
            {
                Id = assignee.UserId,
                FirstName = assignee.FirstName,
                LastName = assignee.LastName,
            };
        }
        var reporter = await userRepository.GetUserById(task.ReporterId);
        taskDto.Reporter = new UserShortInfoDto
        {
            Id = reporter.UserId,
            FirstName = reporter.FirstName,
            LastName = reporter.LastName,
        };
        if (task.Comments != null && task.Comments.Any())
        {
            var commenters = await userRepository.GetUsersByIds(task.Comments.Select(x => x.UserId).ToArray());
            taskDto.Comments = task.Comments.Select(x => new TaskCommentDto
            {
                Content = x.Content,
                Time = x.Time,
                User = commenters.First(us => us.UserId == x.UserId).MapToShortDto()
            }).ToArray();
        }
        return taskDto;
    }
}