using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Repositories
{
    public interface ILessonPackagesRepository
    {
        Task Add(LessonPackage lessonPackage);
        Task<LessonPackage?> GetById(Guid id);
        Task<List<LessonPackage>> GetByStudentId(Guid studentId);
        Task<List<LessonPackage>> GetByTutorId(Guid tutorId);
        Task Update(LessonPackage lessonPackage);
    }
}
