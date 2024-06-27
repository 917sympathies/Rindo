using Rindo.Domain.Common;

namespace Application.Interfaces.Services;

public interface IInvitationService
{
    Task<Result> DeleteInvitation(Guid id);
}