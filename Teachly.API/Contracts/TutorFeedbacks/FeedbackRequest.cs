using System.ComponentModel.DataAnnotations;
using Teachly.Core.Models;

namespace Teachly.API.Contracts.TutorFeedbacks
{
    public record FeedbackRequest(
        [Required]
        Guid SolutionId,
        [Required]
        [Range(2, 5)]
        short Grade,
        [Required]
        [MaxLength(TutorFeedback.MAX_TUTORCOMMENT_LENGTH)]
        string TutorComment);
}
