using Rindo.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Domain.Repositories;

public interface IChatRepository
{
    Task Create(Chat chat);
    Task Delete(Chat chat);
    Task<Chat> GetChatById(Guid id);
}