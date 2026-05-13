using System.ComponentModel.DataAnnotations;
using Teachly.Core.Models;

namespace Teachly.API.Contracts.TutorTasks
{
    public record StudentTaskResponse(
        [Required]
        Guid Id,

        [Required]
        Guid LessonPackageId,

        [Required]
        [MaxLength(TutorTask.MAX_TITLE_LENGTH)]
        string Title,

        [Required]
        [MaxLength(TutorTask.MAX_DESCRIPTION_LENGTH)]
        string Description,

        [Required]
        DateTime CreatedAt,

        DateTime? ClosedAt);
}
