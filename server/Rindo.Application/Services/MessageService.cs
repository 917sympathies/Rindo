using Application.Interfaces.Services;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Rindo.Domain.Services;
using Rindo.Infrastructure;

namespace Application.Services;

public class MessageService : IMessageService
{
    private readonly IChatMessageRepository _messageRepository;
    
    private readonly IUserService _userService;
    
    private readonly PostgresDbContext _context; //TODO: remove DbContext
    
    public MessageService(IChatMessageRepository messageRepository, IUserService userService, PostgresDbContext context)
    {
        _messageRepository = messageRepository;
        _userService = userService;
        _context = context;
    }
    
    public async Task<Tuple<ChatMessage, string>> AddMessage(Guid userId, Guid chatId, string content)
    {
        var user = (await _userService.GetUserById(userId)).Value;
        var msg = new ChatMessage { ChatId = chatId, SenderId = userId, Content = content, Time = DateTime.UtcNow};
        await _messageRepository.AddMessage(msg);
        await _context.SaveChangesAsync();
        return new Tuple<ChatMessage, string>(msg, user!.Username);
    }
}