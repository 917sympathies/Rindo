using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.DataObjects;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class TaskCommentRepository(PostgresDbContext context): RepositoryBase<TaskComment>(context), ITaskCommentRepository
{
    public Task CreateComment(TaskComment comment) => CreateAsync(comment);

    public async Task DeleteComment(TaskComment comment) => await Delete(comment);

    public Task<int> GetCommentsCountByTaskId(Guid taskId)
    {
        var result = FindByCondition(c => c.TaskId == taskId).Count();
        return Task.FromResult(result);
    }
}