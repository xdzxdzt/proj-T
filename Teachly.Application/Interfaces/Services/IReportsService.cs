using Teachly.Application.Reports;

namespace Teachly.Application.Interfaces.Services
{
    public interface IReportsService
    {
        Task<StudentProgressReport> GetStudentProgressReportForStudent(Guid userId);
        Task<StudentProgressReport> GetStudentProgressReportForTutor(Guid userId, Guid studentId);
    }
}
