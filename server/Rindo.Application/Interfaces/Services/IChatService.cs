using Rindo.Domain.DTO;

namespace Application.Interfaces.Services;

public interface IChatService
{
    Task<ChatDto> GetChatById(Guid id);
}