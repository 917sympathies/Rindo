using Application.Interfaces.Services;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure;

namespace Application.Services;

public class CommentService : ICommentService
{
    private readonly ITaskCommentRepository _commentRepository;
    
    private readonly IUserService _userService;
    
    private readonly PostgresDbContext _context; //TODO: remove DbContext
    
    public CommentService(ITaskCommentRepository commentRepository, PostgresDbContext context, IUserService userService)
    {
        _commentRepository = commentRepository;
        _userService = userService;
        _context = context;
    }
    
    public async Task<TaskComment> AddComment(Guid userId, Guid taskId, string content)
    {
        var comment = new TaskComment { TaskId = taskId, UserId = userId, Content = content, Time = DateTime.UtcNow};
        await _commentRepository.CreateComment(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<int> GetCommentsCountByTaskId(Guid taskId)
    {
        var commentsCount = await _commentRepository.GetCommentsCountByTaskId(taskId);
        return commentsCount;
    }
}