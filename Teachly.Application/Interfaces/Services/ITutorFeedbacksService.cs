namespace Teachly.Application.Interfaces.Services
{
    public interface ITutorFeedbacksService
    {
        Task GiveFeedback(Guid tutorId, Guid solutionId, short grade, string tutorComment);
    }
}
