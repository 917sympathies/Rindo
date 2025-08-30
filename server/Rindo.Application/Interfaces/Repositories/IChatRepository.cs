using Rindo.Domain.Models;

namespace Application.Interfaces.Repositories;

public interface IChatRepository
{
    Task<Chat> Create(Chat chat);
    void Delete(Chat chat);
    Task<Chat?> GetChatById(Guid id);
}