using Rindo.Domain.Common;
using Rindo.Domain.DTO;

namespace Application.Interfaces.Services;

public interface IChatService
{
    Task<Result<ChatDto>> GetChatById(Guid id);
}