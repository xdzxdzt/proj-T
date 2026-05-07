using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Repositories
{
    public interface ITutorFeedbacksRepository
    {
        Task Add(TutorFeedback tutorFeedback);
        Task<TutorFeedback?> GetBySolutionId(Guid solutionId);
        Task<bool> ExistsBySolutionId(Guid solutionId);
    }
}
