using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface IChatMessageRepository
{
    Task AddMessage(ChatMessage message);
    Task<IEnumerable<ChatMessage>> GetMessagesFromChat(Guid chatId);
}