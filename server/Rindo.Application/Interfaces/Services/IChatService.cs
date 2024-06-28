using Rindo.Domain.Common;
using Rindo.Domain.Entities;

namespace Application.Interfaces.Services;

public interface IChatService
{
    Task<Result<Chat>> GetChatById(Guid id);
}