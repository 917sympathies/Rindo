using Rindo.Domain.Models;

namespace Application.Interfaces.Services;

public interface IMessageService
{
    Task<Tuple<ChatMessage, string>> AddMessage(Guid userId, Guid chatId, string content);
}