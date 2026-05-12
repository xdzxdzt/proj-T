using System.ComponentModel.DataAnnotations;
using Teachly.Core.Models;

namespace Teachly.API.Contracts.SubmitSolutions
{
    public record SubmitSolutionRequest(
        [Required]
        Guid StudentId,
        [Required]
        Guid TutorTaskId,
        [Required]
        [MaxLength(Solution.MAX_LENGTH_ANSWERTEXT)]
        string AnswerText);
}
