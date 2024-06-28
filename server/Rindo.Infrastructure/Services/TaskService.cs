using Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Common;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;
using ProjectTask = Rindo.Domain.Entities.Task;

namespace Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly RindoDbContext _context;

    public TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository, RindoDbContext context)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _context = context;
    }

    public async Task<Result> CreateTask(ProjectTask task)
    {
        var projectTasks = await _taskRepository.GetTasksByProjectId(task.ProjectId);
        task.Index = projectTasks.Count();
        try
        {
            await _taskRepository.CreateTask(task);
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
        if (task is null) return Result.Failure(Error.NotFound("Такой задачи не существует!"));
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
        if (task is null) return Result.Failure(Error.NotFound("Такой задачи не существует!"));
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

    public async Task<Result> UpdateResponsible(Guid id, Nullable<Guid> userId)
    {
        var task = await _taskRepository.GetById(id);
        if (task is null) return Result.Failure(Error.NotFound("Такой задачи не существует!"));
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
        if (task is null) return Result.Failure(Error.NotFound("Такой задачи не существует!"));
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
        if (task is null) return Result.Failure(Error.NotFound("Такой задачи не существует!"));
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
        if (task is null) return Result.Failure(Error.NotFound("Такой задачи не существует!"));
        if (!int.TryParse(number, out var num)) return Result.Failure(Error.Failure("Это не число!"));
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

    public async Task UpdateTask(ProjectTask task)
    {
        await _taskRepository.UpdateTask(task);
        await _context.SaveChangesAsync();
    }

    public async Task<Result> DeleteTask(Guid id)
    {
        var task = await _taskRepository.GetById(id);
        if (task is null) return Result.Failure(Error.NotFound("Такой задачи не существует!"));
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

    public async Task<Result<ProjectTask?>> GetTaskById(Guid id)
    {
        var task = await _taskRepository.GetById(id);
        if (task is null) return Error.NotFound("Такой задачи не существует!");
        return task;
    }
}