using Application.Interfaces.Services;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;

namespace Application.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    
    private readonly RindoDbContext _context;
        
    public ChatService(IChatRepository chatRepository, RindoDbContext context)
    {
        _chatRepository = chatRepository;
        _context = context;
    }
    
    public async Task<Result<ChatDto>> GetChatById(Guid id)
    {
        var chat = await _chatRepository.GetChatById(id);
        if (chat is null) return Error.NotFound("Chat with this id doesn't exists");
        
        return new ChatDto
        {
            Id = chat.Id,
            Messages = chat.Messages.Select(x => new MessageDto
            {
                Id = x.Id,
                ChatId = x.ChatId,
                Content = x.Content,
                Time = x.Time,
                Username = _context.Users.FirstOrDefault(user => user.Id == x.SenderId)!.Username
            }).ToArray()
        };
    }
}