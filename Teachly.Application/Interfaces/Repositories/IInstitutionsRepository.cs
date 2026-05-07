using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Repositories
{
    public interface IInstitutionsRepository
    {
        Task Add(Institution institution);
        Task<Institution?> GetById(Guid id);
        Task<List<Institution>> GetAll();
    }
}
