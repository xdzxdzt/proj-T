using System.ComponentModel.DataAnnotations;

namespace Teachly.API.Contracts.Reports
{
    public record StudentProgressReportResponse(
        [Required]
        Guid StudentId,

        [Required]
        [Range(0, int.MaxValue)]
        int SubmittedSolutions,

        [Required]
        [Range(0, int.MaxValue)]
        int CheckedSolutions,

        [Required]
        [Range(0, 5)]
        double AverageGrade);
}
