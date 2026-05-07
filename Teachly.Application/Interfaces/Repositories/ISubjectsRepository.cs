using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Repositories
{
    public interface ISubjectsRepository
    {
        Task Add(Subject subject);
        Task<Subject?> GetById(Guid id);
        Task<Subject?> GetByName(string name);
        Task<List<Subject>> GetAll();
    }
}
