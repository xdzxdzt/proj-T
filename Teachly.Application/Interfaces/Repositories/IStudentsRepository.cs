using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Repositories
{
    public interface IStudentsRepository
    {
        Task Add(Student student);
        Task<bool> ExistsByUserId(Guid userId);
        Task<Student?> GetById(Guid id);
        Task<Student?> GetByUserId(Guid userId);
    }
}