namespace Teachly.Application.Interfaces.Services
{
    public interface ITutorSubjectService
    {
        Task AddSubjectToTutor(Guid tutorId, Guid subjectId, decimal pricePerLesson);
    }
}
