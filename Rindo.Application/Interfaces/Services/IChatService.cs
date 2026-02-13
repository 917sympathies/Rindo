using Rindo.Domain.DTO;
using Rindo.Domain.DTO.Projects;
using Rindo.Domain.DataObjects;

namespace Application.Interfaces.Services;

public interface IChatService
{
    Task<Guid> Create(Chat chat);
    Task DeleteById(Guid chatId);
    Task<ChatDto> GetChatById(Guid id);
}