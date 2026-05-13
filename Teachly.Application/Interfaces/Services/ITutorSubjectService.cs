namespace Teachly.Application.Interfaces.Services
{
    public interface ITutorSubjectService
    {
        Task AddSubjectToTutor(Guid userId, Guid subjectId, decimal pricePerLesson);
    }
}
