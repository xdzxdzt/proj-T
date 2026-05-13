namespace Teachly.Application.Interfaces.Services
{
    public interface ITutorFeedbacksService
    {
        Task GiveFeedback(Guid userId, Guid solutionId, short grade, string tutorComment);
    }
}
