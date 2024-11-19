using Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Common;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    
    private readonly RindoDbContext _context;

    public TaskService(ITaskRepository taskRepository, RindoDbContext context)
    {
        _taskRepository = taskRepository;
        _context = context;
    }

    public async Task<Result> CreateTask(ProjectTask projectTask)
    {
        var projectTasks = await _taskRepository.GetTasksByProjectId(projectTask.ProjectId);
        projectTask.Index = projectTasks.Count();
        try
        {
            await _taskRepository.CreateTask(projectTask);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.Failure(e.Message));
        }
        return Result.Success();
    }

    public async Task<Result> UpdateName(Guid id, string name)
    {
        var task = await _taskRepository.GetById(id);
        if (task is null) return Result.Failure(Error.NotFound("Task with this id doesn't exists"));
        task.Name = name;
        try
        {
            await _taskRepository.UpdateProperty(task, t => t.Name);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.Failure(e.Message));
        }
        return Result.Success();
    }

    public async Task<Result> UpdateDescription(Guid id, string description)
    {
        var task = await _taskRepository.GetById(id);
        if (task is null) return Result.Failure(Error.NotFound("Task with this id doesn't exists"));
        task.Description = description;
        try
        {
            await _taskRepository.UpdateProperty(task, t => t.Description);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.Failure(e.Message));
        }
        return Result.Success();
    }

    public async Task<Result> UpdateResponsible(Guid id, Guid? userId)
    {
        var task = await _taskRepository.GetById(id);
        if (task is null) return Result.Failure(Error.NotFound("Task with this id doesn't exists"));
        task.ResponsibleUserId = userId;
        try
        {
            await _taskRepository.UpdateTask(task);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.Failure(e.Message));
        }
        return Result.Success();
    }

    public async Task<Result> UpdateStartDate(Guid id, DateOnly date)
    {
        var task = await _taskRepository.GetById(id);
        if (task is null) return Result.Failure(Error.NotFound("Task with this id doesn't exists"));
        task.StartDate = date;
        try
        {
            await _taskRepository.UpdateProperty(task, t => t.StartDate);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.Failure(e.Message));
        }
        return Result.Success();
    }

    public async Task<Result> UpdateFinishDate(Guid id, DateOnly date)
    {
        var task = await _taskRepository.GetById(id);
        if (task is null) return Result.Failure(Error.NotFound("Task with this id doesn't exists"));
        task.FinishDate = date;
        try
        {
            await _taskRepository.UpdateProperty(task, t => t.FinishDate);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.Failure(e.Message));
        }
        return Result.Success();
    }

    public async Task<Result> UpdateProgress(Guid id, string number)
    {
        var task = await _taskRepository.GetById(id);
        if (task is null) return Result.Failure(Error.NotFound("Task with this id doesn't exists"));
        if (!int.TryParse(number, out var num)) return Result.Failure(Error.Failure("Invalid number"));
        task.Progress = num;
        try
        {
            await _taskRepository.UpdateProperty(task, t => t.Progress);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.Failure(e.Message));
        }
        return Result.Success();
    }

    public async Task UpdateTask(ProjectTask projectTask)
    {
        await _taskRepository.UpdateTask(projectTask);
        await _context.SaveChangesAsync();
    }

    public async Task<Result> DeleteTask(Guid id)
    {
        var task = await _taskRepository.GetById(id);
        if (task is null) return Result.Failure(Error.NotFound("Task with this id doesn't exists"));
        try
        {
            await _taskRepository.DeleteTask(task);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.Failure(e.Message));
        }
        return Result.Success();
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksByStageId(Guid stageId)
    {
        return await _taskRepository.GetTasksByStageId(stageId);
    }

    public async Task<IEnumerable<object>> GetTasksByProjectId(Guid projectId)
    {
        var tasks = await _context.Tasks.Where(task => task.ProjectId == projectId).ToListAsync();
        var result = tasks.Select(t => new
            { task = t, user = _context.Users.FirstOrDefault(u => u.Id == t.ResponsibleUserId) });
        return result;
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksByUserId(Guid userId)
    {
        var tasks = await _taskRepository.GetTasksByUserId(userId);
        return tasks;
    }

    public async Task<Result<object>> GetTaskById(Guid id)
    {
        var task = await _taskRepository.GetById(id);
        if (task is null) return Error.NotFound($"Task with this id doesn't exists {id}");
        var comments = _context.TaskComments.Where(cm => cm.TaskId == task.Id).Select(cm =>
            new
            {
                cm.Id, cm.Content, cm.TaskId, cm.UserId, cm.Time, username = _context.Users.FirstOrDefault(user => user.Id == cm.UserId)!.Username
            }).ToList();
        return new { task, comments };
    }
}