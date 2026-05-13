namespace Teachly.Application.Reports
{
    public record StudentProgressReport(
        Guid StudentId,
        int SubmittedSolutions,
        int CheckedSolutions,
        double AverageGrade);
}
