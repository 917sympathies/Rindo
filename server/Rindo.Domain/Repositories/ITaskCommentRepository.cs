using Rindo.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;

public interface ITaskCommentRepository
{
    Task CreateComment(TaskComment comment);
    Task DeleteComment(TaskComment comment);
    Task<int> GetCommentsCountByTaskId(Guid taskId);
}