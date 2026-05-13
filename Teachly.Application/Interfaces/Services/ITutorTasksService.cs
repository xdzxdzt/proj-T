using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Services
{
    public interface ITutorTasksService
    {
        Task CreateTask(Guid userId, Guid lessonPackageId, string title, string description);
        Task<List<TutorTask>> GetByLessonPackageId(Guid userId, Guid lessonPackageId);
        Task<List<TutorTask>> GetForStudent(Guid userId);
        Task CloseTask(Guid userId, Guid taskId);
    }
}
