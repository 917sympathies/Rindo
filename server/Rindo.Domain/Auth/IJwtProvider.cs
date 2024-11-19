using Rindo.Domain.Models;

namespace Rindo.Infrastructure;

public interface IJwtProvider
{
    string GenerateToken(User user);
}