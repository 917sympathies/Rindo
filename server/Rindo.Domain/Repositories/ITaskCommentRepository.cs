using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;

public interface ITaskCommentRepository
{
    Task CreateComment(TaskComment comment);
    void DeleteComment(TaskComment comment);
    Task<int> GetCommentsCountByTaskId(Guid taskId);
}