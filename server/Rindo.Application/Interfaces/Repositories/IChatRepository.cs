using Rindo.Domain.DataObjects;

namespace Application.Interfaces.Repositories;

public interface IChatRepository
{
    Task<Chat> Create(Chat chat);
    Task DeleteById(Guid chatId);
    Task<Chat?> GetChatById(Guid id);
}