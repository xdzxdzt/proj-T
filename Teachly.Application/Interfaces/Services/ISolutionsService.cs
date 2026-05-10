namespace Teachly.Application.Interfaces.Services
{
    public interface ISolutionsService
    {
        Task SubmitSolution(Guid studentId, Guid tutorTaskId, string answerText);
    }
}
