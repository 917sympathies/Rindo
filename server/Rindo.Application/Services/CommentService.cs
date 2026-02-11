using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Rindo.Domain.DataObjects;

namespace Application.Services;

public class CommentService(ITaskCommentRepository commentRepository, IUserService userService) : ICommentService
{
    private readonly IUserService _userService = userService;

    public async Task<TaskComment> AddComment(Guid userId, Guid taskId, string content)
    {
        var comment = new TaskComment { TaskId = taskId, UserId = userId, Content = content, Time = DateTime.UtcNow};
        await commentRepository.CreateComment(comment);
        return comment;
    }

    public async Task<int> GetCommentsCountByTaskId(Guid taskId)
    {
        var commentsCount = await commentRepository.GetCommentsCountByTaskId(taskId);
        return commentsCount;
    }
}