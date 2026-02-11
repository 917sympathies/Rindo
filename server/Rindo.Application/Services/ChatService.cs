using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Projects;
using Rindo.Domain.DataObjects;

namespace Application.Services;

public class ChatService(IChatRepository chatRepository, IUserRepository userRepository) : IChatService
{
    public async Task<Guid> Create(Chat chat)
    {
        var createdChat = await chatRepository.Create(chat);
        return createdChat.ChatId;
    }

    public async Task DeleteById(Guid chatId)
    {
        await chatRepository.DeleteById(chatId);
    }
    
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
                Id = message.MessageId,
                ChatId = message.ChatId,
                Content = message.Content,
                Time = message.Time,
                Username = user.Username,
            });
        }
        
        return new ChatDto
        {
            Id = chat.ChatId,
            Messages = messages.ToArray()
        };
    }
    
}