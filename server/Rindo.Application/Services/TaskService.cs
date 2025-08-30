using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Common;
using Rindo.Domain.Models;
using Rindo.Infrastructure;
using Task = System.Threading.Tasks.Task;
using TaskStatus = Rindo.Domain.Enums.TaskStatus;

namespace Application.Services;

public class TaskService(ITaskRepository taskRepository) : ITaskService
{
    public async Task<ProjectTask> CreateTask(ProjectTask projectTask)
    {
        var projectTasks = await taskRepository.GetTasksByProjectId(projectTask.ProjectId);
        projectTask.Index = projectTasks.Count();
        return await taskRepository.CreateTask(projectTask);
    }

    public async Task UpdateName(Guid taskId, string name)
    {
        var task = await taskRepository.GetById(taskId);
        if (task is null) throw new NotFoundException(nameof(Task), taskId);
        task.Name = name;
        await taskRepository.UpdateProperty(task, t => t.Name);
    }

    public async Task UpdateDescription(Guid taskId, string description)
    {
        var task = await taskRepository.GetById(taskId);
        if (task is null) throw new NotFoundException(nameof(Task), taskId);
        task.Description = description;
        await taskRepository.UpdateProperty(task, t => t.Description);
    }

    public async Task UpdateResponsible(Guid taskId, Guid? userId)
    {
        var task = await taskRepository.GetById(taskId);
        if (task is null) throw new NotFoundException(nameof(Task), taskId);
        task.AsigneeId = userId;
        taskRepository.UpdateTask(task);
    }

    public async Task UpdateStartDate(Guid taskId, DateOnly date)
    {
        var task = await taskRepository.GetById(taskId);
        if (task is null) throw new NotFoundException(nameof(Task), taskId);
        task.CreatedDate = date;
        await taskRepository.UpdateProperty(task, t => t.CreatedDate);
    }

    public async Task UpdateFinishDate(Guid taskId, DateOnly date)
    {
        var task = await taskRepository.GetById(taskId);
        if (task is null) throw new NotFoundException(nameof(Task), taskId);
        task.UpdatedDate = date;
        await taskRepository.UpdateProperty(task, t => t.UpdatedDate);
    }

    public async Task UpdateProgress(Guid taskId, string number)
    {
        var task = await taskRepository.GetById(taskId);
        if (task is null) throw new NotFoundException(nameof(Task), taskId);
        if (!int.TryParse(number, out var num)) throw new ArgumentException(nameof(number), number);
        task.Status = (TaskStatus)num;
        await taskRepository.UpdateProperty(task, t => t.Status);
    }

    public void UpdateTask(ProjectTask projectTask)
    {
        taskRepository.UpdateTask(projectTask);
    }

    public async Task DeleteTask(Guid taskId)
    {
        var task = await taskRepository.GetById(taskId);
        if (task is null) throw new NotFoundException(nameof(Task), taskId);
        taskRepository.DeleteTask(task);
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksByStageId(Guid stageId)
    {
        return await taskRepository.GetTasksByStageId(stageId);
    }

    public async Task<IEnumerable<object>> GetTasksByProjectId(Guid projectId)
    {
        var tasks = await taskRepository.GetTasksByProjectId(projectId);
        // var result = tasks.Select(t => new
        //     { task = t, user = _context.Users.FirstOrDefault(u => u.Id == t.AsigneeId) });
        // return result;
        return Array.Empty<object>();
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksByUserId(Guid userId)
    {
        var tasks = await taskRepository.GetTasksByUserId(userId);
        return tasks;
    }

    public async Task<object> GetTaskById(Guid id)
    {
        var task = await taskRepository.GetById(id);
        if (task is null) return Error.NotFound($"Task with this id doesn't exists {id}");
        // var comments = _context.TaskComments.Where(cm => cm.TaskId == task.Id).Select(cm =>
        //     new
        //     {
        //         cm.Id, cm.Content, cm.TaskId, cm.UserId, cm.Time, username = _context.Users.FirstOrDefault(user => user.Id == cm.UserId)!.Username
        //     }).ToList();
        return new { };
    }
}