using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Services
{
    public interface ITutorTasksService
    {
        Task CreateTask(Guid tutorId, Guid lessonPackageId, string title, string description);
        Task<List<TutorTask>> GetByLessonPackageId(Guid lessonPackageId);
        Task CloseTask(Guid tutorId, Guid taskId);
    }
}
