﻿using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class TaskCommentRepository : RepositoryBase<TaskComment>, ITaskCommentRepository
{
    public TaskCommentRepository(PostgresDbContext context) : base(context)
    {
    }

    public Task CreateComment(TaskComment comment) => CreateAsync(comment);

    public void DeleteComment(TaskComment comment) => Delete(comment);

    public Task<int> GetCommentsCountByTaskId(Guid taskId)
    {
        var result = FindByCondition(c => c.TaskId == taskId).Count();
        return Task.FromResult(result);
    }
}