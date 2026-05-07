using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Repositories
{
    public interface ITutorTasksRepository
    {
        Task Add(TutorTask task);
        Task<TutorTask?> GetById(Guid id);
        Task<List<TutorTask>> GetByLessonPackageId(Guid lessonPackageId);
        Task Update(TutorTask task);
    }
}
