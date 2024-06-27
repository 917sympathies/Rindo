using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class TaskCommentRepository : RepositoryBase<TaskComment>, ITaskCommentRepository
{
    public TaskCommentRepository(RindoDbContext context) : base(context)
    {
    }

    public Task CreateComment(TaskComment comment) => CreateAsync(comment);

    public Task DeleteComment(TaskComment comment) => DeleteAsync(comment);

    public Task<int> GetCommentsCountByTaskId(Guid taskId)
    {
        var result = FindByCondition(c => c.TaskId == taskId).Count();
        return Task.FromResult(result);
    }
}