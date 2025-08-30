using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Rindo.Domain.Models;

namespace Application.Services;

public class MessageService(IChatMessageRepository messageRepository, IUserService userService) : IMessageService
{
    public async Task<Tuple<ChatMessage, string>> AddMessage(Guid userId, Guid chatId, string content)
    {
        var user = (await userService.GetUserById(userId)).Value;
        var msg = new ChatMessage { ChatId = chatId, SenderId = userId, Content = content, Time = DateTime.UtcNow};
        await messageRepository.AddMessage(msg);
        return new (msg, user!.Username);
    }
}