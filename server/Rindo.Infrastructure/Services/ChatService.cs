using Rindo.Domain.Common;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;

namespace Application.Services.ChatService;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository; 
        
    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }
    public async Task<Result<Chat>> GetChatById(Guid id)
    {
        var chat = await _chatRepository.GetChatById(id);
        return chat;
    }
}