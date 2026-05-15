using Teachly.Application.Reports;

namespace Teachly.Application.Interfaces.Services
{
    public interface ILessonPackagesService
    {
        Task BuyPackage(Guid userId, Guid tutorSubjectId, int totalLessons);
        Task UseLesson(Guid userId, Guid lessonPackageId);
        Task<List<LessonPackageInfo>> GetForStudent(Guid userId);
        Task<List<LessonPackageInfo>> GetForTutor(Guid userId);
    }
}
