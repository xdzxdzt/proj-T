using Teachly.Application.DTOs;

namespace Teachly.Application.Interfaces.Services
{
    public interface IReportsService
    {
        Task<StudentProgressReportDto> GetStudentProgressReport(Guid studentId);
    }
}
