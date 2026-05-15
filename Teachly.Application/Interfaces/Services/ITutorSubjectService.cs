using Teachly.Application.Reports;

namespace Teachly.Application.Interfaces.Services
{
    public interface ITutorSubjectService
    {
        Task AddSubjectToTutor(Guid userId, Guid subjectId, decimal pricePerLesson);
        Task<List<TutorSubjectInfo>> GetAll();
        Task<List<TutorSubjectInfo>> GetByTutorId(Guid tutorId);
    }
}
