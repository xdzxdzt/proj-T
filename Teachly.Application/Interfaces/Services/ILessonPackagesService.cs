namespace Teachly.Application.Interfaces.Services
{
    public interface ILessonPackagesService
    {
        Task BuyPackage(Guid studentId, Guid tutorSubjectId, int totalLessons);
        Task UseLesson(Guid lessonPackageId);
    }
}
