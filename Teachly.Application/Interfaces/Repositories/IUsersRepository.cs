using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task Add(User user);
        Task<User?> GetById(Guid id);
        Task<User?> GetByEmail(string email);
        Task<bool> ExistsByEmail(string email);
        Task Update(User user);
    }
}
