using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Services
{
    public interface ISolutionsService
    {
        Task SubmitSolution(Guid userId, Guid tutorTaskId, string answerText);
        Task<List<Solution>> GetAllByStudentId(Guid userId, Guid studentId);
        Task<List<(Solution Solution, TutorTask TutorTask, TutorFeedback? TutorFeedback)>> GetHistoryForStudent(Guid userId);
    }
}
