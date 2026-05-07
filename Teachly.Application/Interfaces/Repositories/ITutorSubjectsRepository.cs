using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Repositories
{
    public interface ITutorSubjectsRepository
    {
        Task Add(TutorSubject tutorSubject);
        Task<TutorSubject?> GetById(Guid id);
        Task<List<TutorSubject>> GetAllByTutorId(Guid tutorId);
        Task<bool> Exists(Guid tutorId, Guid subjectId);
    }
}
