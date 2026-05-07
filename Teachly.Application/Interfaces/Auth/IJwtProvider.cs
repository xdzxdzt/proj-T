using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}