using Application.Interfaces.Services;
using Rindo.Domain.Common;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;

namespace Rindo.Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    
    private readonly RindoDbContext _context;
        
    public ChatService(IChatRepository chatRepository, RindoDbContext context)
    {
        _chatRepository = chatRepository;
        _context = context;
    }
    
    public async Task<Result<object>> GetChatById(Guid id)
    {
        var chat = await _chatRepository.GetChatById(id);
        if (chat is null) return Error.NotFound("Такого чата не существует");
        var messages = chat.Messages.Select(msg => new
        {
            msg.Id, msg.ChatId, msg.Content,
            username = _context.Users.FirstOrDefault(user => user.Id == msg.SenderId)!.Username, msg.Time
        });
        return new {chat.Id,  messages};
    }
}