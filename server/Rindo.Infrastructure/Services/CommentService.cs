using Application.Interfaces.Services;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;

namespace Rindo.Infrastructure.Services;

public class CommentService : ICommentService
{
    private readonly ITaskCommentRepository _commentRepository;
    
    private readonly IUserService _userService;
    
    private readonly RindoDbContext _context;
    
    public CommentService(ITaskCommentRepository commentRepository, RindoDbContext context, IUserService userService)
    {
        _commentRepository = commentRepository;
        _userService = userService;
        _context = context;
    }
    
    public async Task<TaskComment> AddComment(Guid userId, Guid taskId, string content)
    {
        var user = (await _userService.GetUserById(userId)).Value;
        var comment = new TaskComment() { TaskId = taskId, UserId = userId, Content = content, Time = DateTime.UtcNow};
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