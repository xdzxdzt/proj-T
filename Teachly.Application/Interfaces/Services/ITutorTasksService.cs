namespace Teachly.Application.Interfaces.Services
{
    public interface ITutorTasksService
    {
        Task CreateTask(Guid tutorId, Guid lessonPackageId, string title, string description);
        Task CloseTask(Guid tutorId, Guid taskId);
    }
}
