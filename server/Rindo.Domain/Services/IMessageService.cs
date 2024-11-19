using Rindo.Domain.Models;

namespace Rindo.Domain.Services;

public interface IMessageService
{
    Task<Tuple<ChatMessage, string>> AddMessage(Guid userId, Guid chatId, string content);
}