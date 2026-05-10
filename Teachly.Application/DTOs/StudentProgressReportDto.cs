namespace Teachly.Application.DTOs
{
    public record StudentProgressReportDto(
        Guid StudentId,
        int SubmittedSolutions,
        int CheckedSolutions,
        double AverageGrade);
}
