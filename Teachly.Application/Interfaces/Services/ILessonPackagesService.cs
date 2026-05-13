namespace Teachly.Application.Interfaces.Services
{
    public interface ILessonPackagesService
    {
        Task BuyPackage(Guid userId, Guid tutorSubjectId, int totalLessons);
        Task UseLesson(Guid userId, Guid lessonPackageId);
    }
}
