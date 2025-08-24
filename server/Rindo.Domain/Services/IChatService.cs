using Rindo.Domain.DTO;

namespace Rindo.Domain.Services;

public interface IChatService
{
    Task<ChatDto> GetChatById(Guid id);
}