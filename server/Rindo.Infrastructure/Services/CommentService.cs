using Application.Services.UserService;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Application.Services.CommentsService;

public class CommentService : ICommentService
{
    private readonly ITaskCommentRepository _commentRepository;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    
    public CommentService(ITaskCommentRepository commentRepository, IUnitOfWork unitOfWork, IUserService userService)
    {
        _commentRepository = commentRepository;
        _userService = userService;
        _unitOfWork = unitOfWork;
    }
    public async Task<TaskComment> AddComment(Guid userId, Guid taskId, string content)
    {
        var user = (await _userService.GetUserById(userId)).Value;
        var comment = new TaskComment() { TaskId = taskId, UserId = userId, Content = content, Username = user.Username};
        await _commentRepository.CreateComment(comment);
        await _unitOfWork.SaveAsync();
        return comment;
    }

    public async Task<int> GetCommentsCountByTaskId(Guid taskId)
    {
        var commentsCount = await _commentRepository.GetCommentsCountByTaskId(taskId);
        return commentsCount;
    }
}