using Rindo.Domain.Common;
using Rindo.Domain.Entities;

namespace Application.Services.ChatService;

public interface IChatService
{
    Task<Result<Chat>> GetChatById(Guid id);
}