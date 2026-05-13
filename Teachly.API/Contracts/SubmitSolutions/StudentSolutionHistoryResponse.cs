using System.ComponentModel.DataAnnotations;
using Teachly.Core.Models;

namespace Teachly.API.Contracts.SubmitSolutions
{
    public record StudentSolutionHistoryResponse(
        [Required]
        Guid SolutionId,

        [Required]
        Guid TutorTaskId,

        [Required]
        Guid LessonPackageId,

        [Required]
        [MaxLength(TutorTask.MAX_TITLE_LENGTH)]
        string TaskTitle,

        [Required]
        [MaxLength(Solution.MAX_LENGTH_ANSWERTEXT)]
        string AnswerText,

        [Required]
        DateTime SubmittedAt,

        Guid? TutorFeedbackId,

        [Range(2, 5)]
        short? Grade,

        [MaxLength(TutorFeedback.MAX_TUTORCOMMENT_LENGTH)]
        string? TutorComment,

        DateTime? CheckedAt);
}
