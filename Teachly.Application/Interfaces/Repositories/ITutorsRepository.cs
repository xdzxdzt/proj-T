using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Repositories
{
    public interface ITutorsRepository
    {
        Task Add(Tutor tutor);
        Task<Tutor?> GetById(Guid id);
        Task<Tutor?> GetByUserId(Guid userId);
        Task<bool> ExistsByUserId(Guid userId);
        Task Update(Tutor tutor);
    }
}
