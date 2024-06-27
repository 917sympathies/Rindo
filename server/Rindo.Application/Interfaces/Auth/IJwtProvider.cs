using Rindo.Domain.Entities;

namespace Rindo.Infrastructure;

public interface IJwtProvider
{
    string GenerateToken(User user);
}