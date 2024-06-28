using Rindo.Domain.Entities;

namespace Application.Interfaces.Services;

public interface ICommentService
{
    Task<TaskComment> AddComment(Guid userId, Guid taskId, string content);
    Task<int> GetCommentsCountByTaskId(Guid taskId);
}