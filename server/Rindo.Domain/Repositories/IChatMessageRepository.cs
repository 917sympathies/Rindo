using Rindo.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;

public interface IChatMessageRepository
{
    Task AddMessage(ChatMessage message);
    Task<IEnumerable<ChatMessage>> GetMessagesFromChat(Guid chatId);
}