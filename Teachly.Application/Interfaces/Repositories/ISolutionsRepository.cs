using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Repositories
{
    public interface ISolutionsRepository
    {
        Task Add(Solution solution);
        Task<Solution?> GetById(Guid id);
        Task<List<Solution>> GetAllByStudentId(Guid studentId);
        Task<List<Solution>> GetAllByTutorTaskId(Guid tutorTaskId);
    }
}
