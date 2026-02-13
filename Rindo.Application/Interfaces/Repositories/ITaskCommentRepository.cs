using Rindo.Domain.DataObjects;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface ITaskCommentRepository
{
    Task CreateComment(TaskComment comment);
    Task DeleteComment(TaskComment comment);
    Task<int> GetCommentsCountByTaskId(Guid taskId);
}