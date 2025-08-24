using Rindo.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;

public interface IChatRepository
{
    Task<Chat> Create(Chat chat);
    void Delete(Chat chat);
    Task<Chat?> GetChatById(Guid id);
}