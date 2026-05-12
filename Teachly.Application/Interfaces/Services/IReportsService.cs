namespace Teachly.Application.Interfaces.Services
{
    public interface IReportsService
    {
        Task<(Guid StudentId, int SubmittedSolutions, int CheckedSolutions, double AverageGrade)> GetStudentProgressReport(Guid studentId);
    }
}
