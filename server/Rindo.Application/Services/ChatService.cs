using Application.Common.Exceptions;
using Application.Interfaces.Services;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;
using Rindo.Domain.Services;
using Rindo.Infrastructure;

namespace Application.Services;

public class ChatService(IChatRepository chatRepository, IUserRepository userRepository) : IChatService
{
    public async Task<ChatDto> GetChatById(Guid chatId)
    {
        var chat = await chatRepository.GetChatById(chatId);
        if (chat is null) throw new NotFoundException(nameof(Chat), chatId);

        var messages = new List<MessageDto>();
        foreach (var message in chat.Messages)
        {
            var user = await userRepository.GetUserById(message.SenderId);
            messages.Add(new MessageDto 
            {
                Id = message.Id,
                ChatId = message.ChatId,
                Content = message.Content,
                Time = message.Time,
                Username = user.Username,
            });
        }
        
        return new ChatDto
        {
            Id = chat.Id,
            Messages = messages.ToArray()
        };
    }
}